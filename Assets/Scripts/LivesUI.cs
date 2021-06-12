using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    [SerializeField] private GameObject lifeUIPrefab;
    
    private List<GameObject> lifeUIs = new List<GameObject>();

    public void UpdateLivesUI(int lives)
    {
        while (lives > lifeUIs.Count)
        {
            AddLifeUI();
        }
        
        while (lives < lifeUIs.Count)
        {
            RemoveLifeUI();
        }
    }

    private void AddLifeUI()
    {
        GameObject newLifeUI = Instantiate(lifeUIPrefab, transform);
        
        lifeUIs.Add(newLifeUI);
    }

    private void RemoveLifeUI()
    {
        GameObject lifeUI = lifeUIs.Last();

        lifeUIs.Remove(lifeUI);
        
        Destroy(lifeUI);
    }
}
