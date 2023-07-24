using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIControl : MonoBehaviour
{
    [SerializeField] spawner script;
    [SerializeField] AllVariable manager;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] Slider slide;
    [SerializeField] TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        manager.controls.UI.pause.performed += ctx => manager.isPause =! manager.isPause;
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.isPause)
        {
            Time.timeScale = 0;

            pauseMenu.SetActive(true);
        }
        else
        {
            pauseMenu.SetActive(false);
        }

        script.quantitySpawn = (int)slide.value;
        text.text = "# of obstacles " + script.quantitySpawn;
    }

    public void regenerateObstacle()
    {
        script.spawnObstacle();
    }
}
