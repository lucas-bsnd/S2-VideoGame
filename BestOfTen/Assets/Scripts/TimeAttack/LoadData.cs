using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadData : MonoBehaviour
{
    private struct Step
    {
        public float speed;
        public Vector3 destination;
    }

    public Transform capsule;
    private TextAsset CsvFile;
    private List<Step> steps;
    private int currentStep = 0;

    // Start is called before the first frame update
    void Start()
    {
        string[] datas = Directory.GetFiles("./Assets", SceneManager.GetActiveScene().name + "_*.csv");
        if (datas.Length == 0)
        {
            Destroy(gameObject.transform.parent.gameObject);
            return;
        }
        (string, float)[] files = new (string, float)[datas.Length];
        for (int i = 0; i < datas.Length; i++)
        {
            files[i] = (datas[i], float.Parse(GetStringBetweenCharacters(datas[i], '_', '.')));
        }
        SortFiles(files);
        CsvFile = new TextAsset(File.ReadAllText(files[0].Item1));
        for (int i = 1; i < files.Length; i++)
        {
            File.Delete(files[i].Item1);
        }
        ReadCSV();
        capsule.position = steps[0].destination;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((capsule.position - steps[currentStep].destination).sqrMagnitude < 0.1f)
                currentStep = (currentStep + 1) % steps.Count;
        
        capsule.position = Vector3.MoveTowards(capsule.position, steps[currentStep].destination,
                steps[currentStep].speed * Time.deltaTime);
    }

    void ReadCSV()
    {
        steps = new List<Step>();
        string[] snapshot = CsvFile.text.Split('\n');

        for (int i = 0; i < snapshot.Length; i++)
        {
            //Debug.Log(snapshot[i]);
            string[] array = snapshot[i].Split(',');
            steps.Add(
                new Step()
                {
                    speed = float.Parse(array[0].Replace(".", ",")),
                    destination = new Vector3(float.Parse(array[1].Replace(".", ",")),
                        float.Parse(array[2].Replace(".", ",")),
                        float.Parse(array[3].Replace(".", ",")))
                }
                );
        }
    }
    
    private string GetStringBetweenCharacters(string input, char charFrom, char charTo)
    {
        int posFrom = input.IndexOf(charFrom);
        int posTo = input.IndexOf(charTo, posFrom + 1);
        return input.Substring(posFrom + 1, posTo - posFrom - 1);
    }

    private void SortFiles((string, float)[] files)
    {
        int n = files.Length;
        for (int i = 1; i < n; ++i) {
            (string, float) key = files[i];
            int j = i - 1;
            while (j >= 0 && files[j].Item2 > key.Item2)
            {
                files[j + 1] = files[j];
                j = j - 1;
            }
            files[j + 1] = key;
        }
    }
}
