using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cercle : MonoBehaviour
{
    public GameObject Nombres;
    public GameObject Cercles;
    SpriteRenderer CercleColor;
    int n;
    bool enAnimacio = false;

    public void CanviarNombre(Color color, int i){
        CercleColor.color = color;
        n=i;
    }

    public void CanviarOrdre(Sprite nombre){
        Parar();
        foreach(SpriteRenderer sp in Nombres.GetComponentsInChildren<SpriteRenderer>(true)){
            sp.sprite = nombre;
        }
        Cercles.SetActive(false);
        Nombres.SetActive(true);
    }

    public void Parar(){
        enAnimacio = false;
        transform.rotation = Quaternion.identity;
    }

    public void PosarDefault(){
        Parar();
        Cercles.SetActive(true);
        Nombres.SetActive(false);
    }

    public void CompteEnrere(){
        PosarDefault();
        enAnimacio = true;
    }

    // Start is called before the first frame update
    void Awake()
    {
        CercleColor = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && n < Input.touchCount)
        {
            Touch touch = Input.GetTouch(n);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            transform.position = new Vector3(ray.origin.x,ray.origin.y,0);
        }
        if(enAnimacio){
            transform.Rotate(new Vector3(0,0,1));
        }
    }
}
