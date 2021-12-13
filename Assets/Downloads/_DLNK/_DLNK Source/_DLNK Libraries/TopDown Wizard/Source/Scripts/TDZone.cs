using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]

public class TDZone : MonoBehaviour
{
    // set up target floor for new triggers
   [Header("New Trigger Floor Target")]
    public float tritarg = 0f;

    // floors list
    [System.Serializable]
    public class Floor
    {
        public GameObject FloorGO;
        public float FloorHeight;
        public bool allwaysVisible = false;
        public GameObject StaticFloor;
        public GameObject DecoGO;
        public GameObject CShadowGO;
        public GameObject ActiveGO;
        public GameObject LightingGO;
        public GameObject ExteriorDeco;
    }
    [Header("Floors List")]
    public List<Floor> Floors;

    // set new floor name
    [HideInInspector]
    public string florname = "Floor Name";

    // triggers list
    [System.Serializable]
    public class Trigger
    {
        public TDZoneTrigger ZTriger;
        public float TargetFloor;
        public bool isExit;
    }
    [Header("Triggers List")]
    public List<Trigger> Triggers;

    // set trigger parent object
    [HideInInspector]
    public GameObject trigfol;

    //changes required
    [HideInInspector]
    public bool updated;

    private TDScene tdscene;

    public void Start()
    {
        tdscene = GameObject.FindWithTag("TdLevelManager").GetComponent<TDScene>();
        if (Triggers == null)
            Triggers = new List<Trigger>();
        foreach (Floor flo in Floors)
            flo.CShadowGO.gameObject.SetActive(false);
    }
    public void Update()
    {
        foreach (Trigger tri in Triggers)
        {
            // check if any trigger deleted and remove from list
            if (tri.ZTriger == null)
                Triggers.Remove(tri);
            // get trigger properties
            tri.TargetFloor = tri.ZTriger.TargetFloor;
            tri.isExit = tri.ZTriger.exiTrigger;
        }

        foreach (Floor flo in Floors)
        {
            //Check if any floor deleted and remove from list
            if (flo.FloorGO == null)
                Floors.Remove(flo);
            //Make floors static
            if (tdscene.MakeStaticFloors)
            {
                foreach (Transform child in flo.StaticFloor.transform)
                {
                    child.gameObject.isStatic = true;
                    foreach (Transform children in child)
                    {
                        children.gameObject.isStatic = true;
                        foreach (Transform childrens in children)
                        {
                            childrens.gameObject.isStatic = true;
                            foreach (Transform childreno in childrens)
                                childreno.gameObject.isStatic = true;
                        }
                    }

                }
            }
        }
    }
    public void OnDo()
    {
        //Check if list is not initialized
        if (Floors == null)
        {
            Floors = new List<Floor>();
            //create trigger parent
            GameObject florT = new GameObject("Triggers");
            florT.transform.parent = this.transform;
            trigfol = florT;
        }

        //Create core floor childs
        GameObject florG = new GameObject(Floors.Count + " #" + florname);
        GameObject florS = new GameObject("StaticFloor");
        GameObject florD = new GameObject("Decoration");
        GameObject florC = new GameObject("Ceiling Shadow");
        GameObject florA = new GameObject("Active Elements");
        GameObject florL = new GameObject("Lighting");
        //Create aditional floor childs
        GameObject florF = new GameObject("Ground");
        GameObject florW = new GameObject("Walls");
        GameObject florE = new GameObject("Exterior Deco");

        //Set GOs in hierachy
        florG.transform.parent = this.transform;
        florS.transform.parent = florG.transform;
        florD.transform.parent = florG.transform;
        florC.transform.parent = florG.transform;
        florA.transform.parent = florG.transform;
        florL.transform.parent = florG.transform;
        florE.transform.parent = florG.transform;
        florF.transform.parent = florS.transform;
        florW.transform.parent = florS.transform;

        //Add zone to group list
        Floor floorz = new Floor();
        floorz.FloorGO = florG;
        floorz.FloorHeight = Floors.Count;
        floorz.StaticFloor = florS;
        floorz.DecoGO = florD;
        floorz.CShadowGO = florC;
        floorz.ActiveGO = florA;
        floorz.LightingGO = florL;
        floorz.ExteriorDeco = florE;
        Floors.Add(floorz);
    }
    public void OnTrigCreate()
    {
        //Check if list is not initialized
        if (Triggers == null)
            Triggers = new List<Trigger>();

        //create trigger object
        GameObject zontrig = GameObject.CreatePrimitive(PrimitiveType.Cube);
        zontrig.transform.parent = trigfol.transform;
        zontrig.name = "ZoneTrigger";

        //Compose trigger object
        zontrig.AddComponent<TDZoneTrigger>();
        //Set collider trigger
        zontrig.GetComponent<BoxCollider>().isTrigger = enabled;
        //Set material
        zontrig.GetComponent<MeshRenderer>().material = tdscene.ZTriggerMat;
        zontrig.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        //set TDZTrigger properties
        zontrig.GetComponent<TDZoneTrigger>().ParentZone = this;
        zontrig.GetComponent<TDZoneTrigger>().TargetFloor = tritarg;
        zontrig.transform.localScale = new Vector3(1,1,1);

        //add to list
        Trigger trigz = new Trigger();
        trigz.TargetFloor = tritarg;
        trigz.ZTriger = zontrig.GetComponent<TDZoneTrigger>();
        Triggers.Add(trigz);

        //select trigger
        //Selection.activeGameObject = zontrig;

    }
    public void FixedUpdate()
    {
        //TOP DOWN Floor Control

        if (updated && tdscene.tdEnabled)
        {
            foreach (Floor flo in Floors)
            {
                if (tdscene.isInTDZone)
                {
                    //unable static mesh, lights & active elements from upper plants
                    if (flo.FloorHeight > tdscene.ActiveFloor)
                    {
                        flo.StaticFloor.SetActive(false);
                        flo.ExteriorDeco.SetActive(false);
                        flo.LightingGO.SetActive(false);
                        flo.ActiveGO.SetActive(false);
                        flo.DecoGO.SetActive(false);
                        flo.CShadowGO.SetActive(true);
                    }
                    else
                    {
                        flo.StaticFloor.SetActive(true);
                        flo.ExteriorDeco.SetActive(true);
                        flo.LightingGO.SetActive(false);
                        flo.ActiveGO.SetActive(true);
                        flo.DecoGO.SetActive(true);
                        flo.CShadowGO.SetActive(false);
                    }
                    //enable ceiling shadow for actual plant
                    if (flo.FloorHeight == tdscene.ActiveFloor)
                    {
                        flo.CShadowGO.SetActive(true);
                        flo.LightingGO.SetActive(true);
                    }
                    else if (tdscene.OptimizeDeco)
                        flo.DecoGO.SetActive(false);
                    updated = false;
                }
                else
                {
                    flo.StaticFloor.SetActive(true);
                    flo.ExteriorDeco.SetActive(true);
                    flo.ActiveGO.SetActive(true);
                    flo.DecoGO.SetActive(false);
                    flo.CShadowGO.SetActive(false);
                    flo.LightingGO.SetActive(false);
                    updated = false;
                }
            }
        }
        // reactiva todas las plantas al salir del sistema topdown
        else if (updated)
        {
            foreach (Floor flo in Floors)
            {
                flo.StaticFloor.SetActive(true);
                flo.ExteriorDeco.SetActive(true);
                flo.LightingGO.SetActive(true);
                flo.ActiveGO.SetActive(true);
                flo.DecoGO.SetActive(true);
                flo.CShadowGO.SetActive(true);
                updated = false;
            }
        }
    }
    public void OnCeilingTint()
    {
        foreach (Floor flo in Floors)
        {
            foreach (Transform child in flo.CShadowGO.transform)
            {
                child.gameObject.isStatic = false;
                if (child.gameObject.GetComponent<Renderer>())
                child.gameObject.GetComponent<Renderer>().material = tdscene.CeilingShadow;
                foreach (Transform children in child)
                    {
                    children.gameObject.isStatic = false;
                    if (children.gameObject.GetComponent<Renderer>())
                        children.gameObject.GetComponent<Renderer>().material = tdscene.CeilingShadow;
                       foreach (Transform childs in children)
                    {
                        childs.gameObject.isStatic = false;
                        if (childs.gameObject.GetComponent<Renderer>())
                            childs.gameObject.GetComponent<Renderer>().material = tdscene.CeilingShadow;
                    }
                }
            }
        }
    }
}
