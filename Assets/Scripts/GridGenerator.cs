using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    private Camera cam;
    private Vector2 screenBounds;

    private int id = 0;
    [SerializeField] private Box boxPrefab = null;
    private int n = 3;
    private float spawnPosX = 0f;
    private float spawnPosY = 0f;

    private Box[] spawnedBoxes;

    private List<Box> willDeSelect = new List<Box>();
    private List<Box> deSelectL = new List<Box>();

    [Space]
    [SerializeField] private InputField inputField;
    [SerializeField] private Button rebuildBtn;
    [SerializeField] private Text counterTxt;
    private int matchCount = 0;
    
    void Start()
    {
        cam = Camera.main;

        counterTxt.text = "Match Count: " + matchCount;
        rebuildBtn.onClick.AddListener(Rebuild);
    }

    public void Rebuild()
    {
        //If there are any boxes, will be deleted
        if(spawnedBoxes != null && spawnedBoxes.Length != 0)
        {
            for(int i=0; i < spawnedBoxes.Length; i++)
            {
                Destroy(spawnedBoxes[i].gameObject);
            }
        }

        //Refreshing match counter
        matchCount = 0;
        counterTxt.text = "Match Count: " + matchCount;

        //Spawn Boxes
        int n = int.Parse(inputField.text);
        SpawnBox(n);
    }

    private void SpawnBox(int n)
    {
        this.n = n;
        screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));

        //Calculating Box Size
        float boxSize = (screenBounds.x * 2f) / n;

        spawnPosX = screenBounds.x * -1f;
        spawnPosY = screenBounds.y;

        if (boxPrefab != null && n > 1)
        {
            spawnedBoxes = new Box[n * n];
            id = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Box boxClone = Instantiate(boxPrefab, new Vector3(spawnPosX, spawnPosY, 0f), Quaternion.identity, this.transform);
                    boxClone.transform.localScale = new Vector3(boxSize, boxSize, 1f);
                    spawnedBoxes[id] = boxClone;

                    id++;
                    spawnPosX += boxSize;
                }

                spawnPosX = screenBounds.x * -1f;
                spawnPosY -= boxSize;
            }
        }
    }

    public void NeighbourhoodControl()
    {
        for(int i=0; i < spawnedBoxes.Length; i++)
        {
            if (spawnedBoxes[i].GetSelected())
            {
                willDeSelect.Clear();
                willDeSelect.Add(spawnedBoxes[i]);

                if(i - n >= 0)
                {
                    if (spawnedBoxes[i - n].GetSelected()) willDeSelect.Add(spawnedBoxes[i - n]);
                }

                if(i - 1 >= 0 && i % n != 0)
                {
                    if (spawnedBoxes[i - 1].GetSelected()) willDeSelect.Add(spawnedBoxes[i - 1]);
                }

                if(i + 1 < spawnedBoxes.Length && (i + 1) % n != 0)
                {
                    if (spawnedBoxes[i + 1].GetSelected()) willDeSelect.Add(spawnedBoxes[i + 1]);
                }

                if(i + n < spawnedBoxes.Length)
                {
                    if (spawnedBoxes[i + n].GetSelected()) willDeSelect.Add(spawnedBoxes[i + n]);
                }

                
                if(willDeSelect.Count >= 3)
                {
                    for(int k=0; k < willDeSelect.Count; k++)
                    {
                        deSelectL.Add(willDeSelect[k]);
                    }
                }
            }
        }

        if(deSelectL.Count > 0)
        {
            for(int j = 0; j < deSelectL.Count; j++)
            {
                deSelectL[j].DeSelect();
            }

            deSelectL.Clear();

            matchCount++;
            counterTxt.text = "Match Count: " + matchCount;
        }
    }
}
