using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestioTouch : MonoBehaviour
{
    public GameObject Cercle;
    public GameObject Text;
    public List<Sprite> Nombres;
    List<GameObject> Cercles;
    List<Color> Colors;
    bool inici = false;
    int[] ordre = {0,1,2,3,4};

    void Shuffle() {
        int temp;
        int n = Cercles.Count;
        for (int i = 0; i < n - 1; i++) {
            int rnd = Random.Range(i, n);
            temp = ordre[rnd];
            ordre[rnd] = ordre[i];
            ordre[i] = temp;
        }
    }

    IEnumerator PosarDefault(){
        yield return new WaitForSeconds(3f);
        foreach(GameObject cercle in Cercles){
            cercle.GetComponent<Cercle>().PosarDefault();
        }
        inici = false;
    }

    IEnumerator Esperar(){
        yield return new WaitForSeconds(2f);
        Shuffle();
        int i = 0;
        foreach(GameObject cercle in Cercles){
            cercle.GetComponent<Cercle>().CanviarOrdre(Nombres[ordre[i]]);
            i++;
        }
        StartCoroutine(PosarDefault());
    }

    void CompteEnrere(){
        StopAllCoroutines();
        inici = true;
        foreach(GameObject cercle in Cercles){
            cercle.GetComponent<Cercle>().CompteEnrere();
        }
        StartCoroutine(Esperar());
    }

    void GenerarCercles(){
        for(int i=0;i<Input.touchCount;i++){
            if(i<Cercles.Count) continue;
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            
            GameObject c = Instantiate(Cercle,new Vector3(ray.origin.x,ray.origin.y,0f), Quaternion.identity);
            c.GetComponent<Cercle>().CanviarNombre(Colors[i],i);
            Cercles.Add(c);

        }
        foreach(GameObject go in Cercles.GetRange(Input.touchCount,Cercles.Count-Input.touchCount)){
            Destroy(go);
        }
        Cercles.RemoveRange(Input.touchCount,Cercles.Count-Input.touchCount);
        inici = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cercles = new List<GameObject>();
        Color[] _colors = {new Color(1,0.65f,0,1),Color.green,Color.yellow,Color.red,Color.magenta};
        Colors = new List<Color>(_colors);
    }

    // Update is called once per frame
    void Update()
    {
        // Només si hi ha entre 1 i 5 dits
        if (Input.touchCount > 0 && Input.touchCount <= 5)
        {
            Text.SetActive(false);
            // Si s'ha canviat un dels dits, es tornen a generar els cercles
            if(Cercles.Count != Input.touchCount) GenerarCercles();

            // Si no s'havia començat encara i hi ha més d'un dit
            if(!inici && Input.touchCount>1){
                CompteEnrere();
            }else if(Input.touchCount==1){
                StopAllCoroutines();
                inici = false;
                Cercles[0].GetComponent<Cercle>().Parar();
            }
        }
        else if(Input.touchCount == 0){
            Text.SetActive(true);
            if(Cercles.Count>0){
                Destroy(Cercles[0]);
                Cercles.RemoveAt(0);
            }
        }
    }
}
