using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class login : MonoBehaviour
{

    public Button logInBtn;
    public Text txtName;

    // Start is called before the first frame update
    void Start()
    {
     	Button btn = logInBtn.GetComponent<Button>();
		  btn.onClick.AddListener(TaskOnClick);   
    }

    void TaskOnClick(){
        Player.name = txtName.text;
        SceneManager.LoadScene("Play");
	}
}
