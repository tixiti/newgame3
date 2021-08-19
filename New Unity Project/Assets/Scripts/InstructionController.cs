using UnityEngine;

public class InstructionController : MonoBehaviour
{
    public GameObject[] instructionPrefabs;

    private void OnEnable()
    {
        OpenInstruction(GameDataController.instance.currentLevel);
    }

    private void OpenInstruction(int level)
    {
        for (var i = 0; i < instructionPrefabs.Length; i++) instructionPrefabs[i].SetActive(false);
        switch (level)
        {
            case 0:
                instructionPrefabs[0].SetActive(true);
                PlayerPrefs.SetString("isLoadInstruction1","true");
                break;
            case 30:
                instructionPrefabs[1].SetActive(true);
                PlayerPrefs.SetString("isLoadInstruction2","true");
                break;
            case 40:
                instructionPrefabs[2].SetActive(true);
                PlayerPrefs.SetString("isLoadInstruction3","true");
                break;
            case 61:
                instructionPrefabs[3].SetActive(true);
                PlayerPrefs.SetString("isLoadInstruction4","true");
                break;
            case 70:
                instructionPrefabs[4].SetActive(true);
                PlayerPrefs.SetString("isLoadInstruction5","true");
                break;
        }
    }
}