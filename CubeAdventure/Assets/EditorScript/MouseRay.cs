using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MouseRay : MonoBehaviour {

    [SerializeField]
    GameObject CubeParent;
    [SerializeField]
    GameObject PortalParent;
    [SerializeField]
    GameObject NpcParent;
    [SerializeField]
    GameObject EnemyParent;

    public GameObject SelectCube;
    public GameObject SelectNature;
    public GameObject SelectBuild;
    public GameObject SelectBridge;
    public GameObject SelectEnemy;
    public GameObject SelectPortal;
    public GameObject SelectNpc;

    public int objectRotation = 0;
    public int setObjectKind;

    void Start()
    {
       setObjectKind = (int)ObjectKind.CUBE;
    }

    // Update is called once per frame
    void Update () {

        SelectObjectKind();
        ObjectRotate();
        ObjectClick();

	}

    void ObjectClick()      // 큐브 클릭
    {
        if(UICamera.Raycast(Input.mousePosition))  // NGUI의 오브젝트를 클릭하면 뒤의 메인 오브젝트 클릭 방지
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);   // 화면상의 마우스에서 레이를 쏜다. 
        RaycastHit hitInfo;


        if (Physics.Raycast(ray, out hitInfo))  // hitInfo에는 레이가 충돌한 오브젝트의 정보
        {
            if (Input.GetKey(KeyCode.C) && Input.GetMouseButtonDown(0))  // 오브젝트 C + 클릭 시 삭제
            {
                if(hitInfo.transform.gameObject.tag.Equals("Bridge"))
                {
                    Destroy(hitInfo.transform.parent.gameObject);
                }
                else
                {
                    Destroy(hitInfo.transform.gameObject);
                }
            }
            else if (hitInfo.transform.tag.Equals("Cube") && Input.GetMouseButtonDown(0))
            {
                switch(setObjectKind)
                {
                    case (int)ObjectKind.CUBE:
                        {
                            CreateCube(hitInfo);
                            break;
                        }
                    case (int)ObjectKind.NATURE:
                        {
                            CreateNature(hitInfo);
                            break;
                        }
                    case (int)ObjectKind.BUILD:
                        {
                            CreateBuild(hitInfo);
                            break;
                        }
                    case (int)ObjectKind.BRIDGE:
                        {
                            CreateBridge(hitInfo);
                            break;
                        }
                    case (int)ObjectKind.FURNITURE:
                        {
                            break;
                        }
                    case (int)ObjectKind.ENEMY:
                        {
                            CreateEnemy(hitInfo);
                            break;
                        }
                    case (int)ObjectKind.PORTAL:
                        {
                            CreatePortal(hitInfo);
                            break;
                        }
                    case (int)ObjectKind.NPC:
                        {
                            CreateNpc(hitInfo);
                            break;
                        }
                }   
            }
        }
    }

    void SelectObjectKind()       // 팔레트 비활성화/활성화
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && setObjectKind != (int)ObjectKind.CUBE)
        {
            setObjectKind = (int)ObjectKind.CUBE;
            UIManager.Instance.SetPallete(setObjectKind);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) && setObjectKind != (int)ObjectKind.NATURE)
        {
            setObjectKind = (int)ObjectKind.NATURE;
            UIManager.Instance.SetPallete(setObjectKind);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3) && setObjectKind != (int)ObjectKind.BUILD)
        {
            setObjectKind = (int)ObjectKind.BUILD;
            UIManager.Instance.SetPallete(setObjectKind);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && setObjectKind != (int)ObjectKind.BRIDGE)
        {
            setObjectKind = (int)ObjectKind.BRIDGE;
            UIManager.Instance.SetPallete(setObjectKind);
        }
        /*  if(Input.GetKeyDown(KeyCode.Alpha5) && setObjectKind != (int)EditorEnum.ObjectKind.FURNITURE)
          {
              setObjectKind = (int)EditorEnum.ObjectKind.FURNITURE;
          }
          */
        if (Input.GetKeyDown(KeyCode.Alpha6) && setObjectKind != (int)ObjectKind.ENEMY)
        {
            setObjectKind = (int)ObjectKind.ENEMY;
            UIManager.Instance.SetPallete(setObjectKind);
        }

        if(Input.GetKeyDown(KeyCode.Alpha7) && setObjectKind != (int)ObjectKind.PORTAL)
        {
            setObjectKind = (int)ObjectKind.PORTAL;
            UIManager.Instance.SetPallete(setObjectKind);
        }

        if(Input.GetKeyDown(KeyCode.Alpha8) && setObjectKind != (int)ObjectKind.NPC)
        {
            setObjectKind = (int)ObjectKind.NPC;
            UIManager.Instance.SetPallete(setObjectKind);
        }
    }

    void ObjectRotate()       // Q,E 키입력을 오브젝트 회전값 설정
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            objectRotation += 15;
            if(objectRotation >= 360)
            {
                objectRotation = 0;
            }
            UIManager.Instance.AngleLabelUpdate(objectRotation);
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            objectRotation -= 15;
            if(objectRotation <= 0)
            {
                objectRotation = 0;
            }
            UIManager.Instance.AngleLabelUpdate(objectRotation);
        }
    }

    public void ObjectSelect(string palleteObjectName)     // 팔레트 오브젝트 클릭
    {
        switch(setObjectKind)
        {
            case (int)ObjectKind.CUBE:
                {
                    SelectCube = Resources.Load("Object/" + palleteObjectName) as GameObject;
                    this.GetComponent<SmartCursor>().selectObject = SelectCube;
                    this.GetComponent<SmartCursor>().CursorChange();
                    break;
                }
            case (int)ObjectKind.NATURE:
                {
                    SelectNature = Resources.Load("Object/" + palleteObjectName) as GameObject;
                    this.GetComponent<SmartCursor>().selectObject = SelectNature;
                    this.GetComponent<SmartCursor>().CursorChange();
                    break;
                }
            case (int)ObjectKind.BUILD:
                {
                    SelectBuild = Resources.Load("Object/" + palleteObjectName) as GameObject;
                    this.GetComponent<SmartCursor>().selectObject = SelectBuild;
                    this.GetComponent<SmartCursor>().CursorChange();
                    break;
                }
            case (int)ObjectKind.BRIDGE:
                {
                    SelectBridge = Resources.Load("Object/" + palleteObjectName) as GameObject;
                    this.GetComponent<SmartCursor>().selectObject = SelectBridge;
                    this.GetComponent<SmartCursor>().CursorChange();
                    break;
                }
            case (int)ObjectKind.ENEMY:
                {
                    SelectEnemy = Resources.Load("Character/" + palleteObjectName) as GameObject;
                    this.GetComponent<SmartCursor>().selectObject = SelectEnemy;
                    this.GetComponent<SmartCursor>().CursorChange();
                    break;
                }
            
            case (int)ObjectKind.PORTAL:
                {
                    SelectPortal = Resources.Load("Object/" + palleteObjectName) as GameObject;
                    this.GetComponent<SmartCursor>().selectObject = SelectPortal;
                    this.GetComponent<SmartCursor>().CursorChange();
                    break;
                }
            case (int)ObjectKind.NPC:
                {
                    SelectNpc = Resources.Load("Character/" + palleteObjectName) as GameObject;
                    this.GetComponent<SmartCursor>().selectObject = SelectNpc;
                    this.GetComponent<SmartCursor>().CursorChange();
                    break;
                }
    }
}
    

    void CreateCube(RaycastHit hitInfo)  // 큐브 설치
    {
        Vector3 v3RelativePoint = hitInfo.transform.InverseTransformPoint(hitInfo.point);
        Vector3 v3HitCube = hitInfo.transform.position;

        GameObject newCube = null;

        if (v3RelativePoint.x >= 0.99f)
        {
            //Debug.Log("오른쪽");
            newCube = Instantiate(SelectCube, v3HitCube + (Vector3.right * 2), Quaternion.identity);
        }
        else if (v3RelativePoint.x <= -0.99f)
        {
            //Debug.Log("왼쪽");
            newCube = Instantiate(SelectCube, v3HitCube + (Vector3.left * 2), Quaternion.identity);
        }
        else if (v3RelativePoint.y >= 0.99f)
        {
            //Debug.Log("위쪽");
            newCube = Instantiate(SelectCube, v3HitCube + (Vector3.up * 2), Quaternion.identity);
        }
        else if (v3RelativePoint.y <= -0.99f)
        {
            //Debug.Log("아래쪽");
            newCube = Instantiate(SelectCube, v3HitCube + (Vector3.down * 2), Quaternion.identity);
        }
        else if (v3RelativePoint.z >= 0.99f)
        {
            //Debug.Log("앞쪽");
            newCube = Instantiate(SelectCube, v3HitCube + (Vector3.forward * 2), Quaternion.identity);
        }
        else if (v3RelativePoint.z <= -0.99f)
        {
            //Debug.Log("뒤쪽");
            newCube = Instantiate(SelectCube, v3HitCube + (Vector3.back * 2), Quaternion.identity);
        }
        newCube.transform.parent = CubeParent.transform;
    }

    void CreateNature(RaycastHit hitInfo) // 자연물 설치
    {
        Vector3 v3RelativePoint = hitInfo.transform.InverseTransformPoint(hitInfo.point);
        Vector3 v3HitCube = hitInfo.transform.position;

        if(v3RelativePoint.y >= 0.99f)
        {
            GameObject newNature = null;
            newNature = Instantiate(SelectNature, hitInfo.point, Quaternion.identity);
            newNature.transform.eulerAngles = new Vector3(newNature.transform.localEulerAngles.x, objectRotation, newNature.transform.localEulerAngles.z);
            newNature.transform.parent = CubeParent.transform;
            switch(SelectNature.name)
            {
                case "ForestGrass01":
                    {
                        break;
                    }
                case "ForestGrass02":
                    {
                        break;
                    }
                case "ForestTreeAppleShort":
                    {
                        break;
                    }
                case "ForestTreeAppleTall":
                    {
                        break;
                    }
                case "ForestTreeDShort":
                    {
                        break;
                    }
                case "ForestTreeDTall":
                    {
                        break;
                    }
                case "ForestTreePineShort":
                    {
                        newNature.transform.position = new Vector3(newNature.transform.position.x, v3HitCube.y + 2.054519f, newNature.transform.position.z);
                        break;
                    }
                case "ForestTreePineShort2":
                    {
                        newNature.transform.position = new Vector3(newNature.transform.position.x, v3HitCube.y + 3.109039f, newNature.transform.position.z);
                        break;
                    }
                case "ForestTreePineTall":
                    {
                        newNature.transform.position = new Vector3(newNature.transform.position.x, v3HitCube.y + 2.054519f, newNature.transform.position.z);
                        break;
                    }
                case "ForestTreePineTall2":
                    {
                        newNature.transform.position = new Vector3(newNature.transform.position.x, v3HitCube.y + 3.109039f, newNature.transform.position.z);
                        break;
                    }
                case "DesertCactus01":
                    {
                        newNature.transform.position = new Vector3(newNature.transform.position.x, v3HitCube.y + 1.3f, newNature.transform.position.z);
                        break;
                    }
                case "DesertCactus02":
                    {
                        newNature.transform.position = new Vector3(newNature.transform.position.x, v3HitCube.y + 1.3f, newNature.transform.position.z);
                        break;
                    }
            }
        }
    }

    void CreateBuild(RaycastHit hitInfo)    // 건물 설치
    {
        Vector3 v3RelativePoint = hitInfo.transform.InverseTransformPoint(hitInfo.point);
        Vector3 v3HitCube = hitInfo.transform.position;

        if (v3RelativePoint.y >= 0.99f)
        {
            GameObject newBuild = null;
            newBuild = Instantiate(SelectBuild, hitInfo.point, Quaternion.identity);
            newBuild.transform.localEulerAngles = new Vector3(newBuild.transform.localEulerAngles.x, objectRotation, newBuild.transform.localEulerAngles.z);
            newBuild.transform.parent = CubeParent.transform;
            switch (SelectBuild.name)
            {
                case "ForestBrazzierBlue":
                    {
                        newBuild.transform.position = new Vector3(newBuild.transform.position.x, v3HitCube.y + 1.96f, newBuild.transform.position.z);
                        break;
                    }
                case "ForestCastle_Blue":
                    {
                        break;
                    }
                case "ForestTower_Blue":
                    {
                        break;
                    }
                case "DesertBrazzier_Blue":
                    {
                        newBuild.transform.position = new Vector3(newBuild.transform.position.x, v3HitCube.y + 2.304677f, newBuild.transform.position.z);
                        break;
                    }
                case "DesertCastle_Blue":
                    {
                        break;
                    }
                case "DesertFlag_Blue":
                    {
                        break;
                    }
                case "DesertTower_Blue":
                    {
                        newBuild.transform.position = new Vector3(newBuild.transform.position.x, v3HitCube.y + 4.095812f, newBuild.transform.position.z);

                        break;
                    }

            }
        }
    }

    void CreateBridge(RaycastHit hitInfo)
    {
        Vector3 v3RelativePoint = hitInfo.transform.InverseTransformPoint(hitInfo.point);
        Vector3 v3HitCube = hitInfo.transform.position;

        if (v3RelativePoint.y >= 0.99f)
        {
            GameObject newBridge = null;
            newBridge = Instantiate(SelectBridge, v3HitCube, Quaternion.identity);
            newBridge.transform.localEulerAngles = new Vector3(newBridge.transform.localEulerAngles.x, objectRotation, newBridge.transform.localEulerAngles.z);
            newBridge.transform.parent = CubeParent.transform;
            switch (SelectBridge.name)
            {
                case "DesertBridgeEnd":
                    {
                        newBridge.transform.position = new Vector3(newBridge.transform.position.x, v3HitCube.y + 1.40656f, newBridge.transform.position.z);
                        break;
                    }
                case "DesertBridgeMainW":
                    {
                        newBridge.transform.position = new Vector3(newBridge.transform.position.x, v3HitCube.y + 1.419906f, newBridge.transform.position.z);

                        break;
                    }
                case "DesertBridgeExtra01":
                    {
                        newBridge.transform.position = new Vector3(newBridge.transform.position.x, v3HitCube.y + 1.419906f, newBridge.transform.position.z);
                        break;
                    }
                case "DesertBridgeExtra01W":
                    {
                        newBridge.transform.position = new Vector3(newBridge.transform.position.x, v3HitCube.y + 1.419906f, newBridge.transform.position.z);

                        break;
                    }
                case "DesertBridgeEndW":
                    {
                        newBridge.transform.position = new Vector3(newBridge.transform.position.x, v3HitCube.y + 1.40656f, newBridge.transform.position.z);

                        break;
                    }
                case "DesertBridgeMain":
                    {
                        newBridge.transform.position = new Vector3(newBridge.transform.position.x, v3HitCube.y + 1.419906f, newBridge.transform.position.z);

                        break;
                    }
                case "ForestBridgeRegular":
                    {
                        newBridge.transform.position = new Vector3(newBridge.transform.position.x, v3HitCube.y + 1.1666f, newBridge.transform.position.z);

                        break;
                    }
                case "ForestBridgeWide":
                    {
                        newBridge.transform.position = new Vector3(newBridge.transform.position.x, v3HitCube.y + 1.1666f, newBridge.transform.position.z);

                        break;
                    }

            }
        }
    }

    void CreateEnemy(RaycastHit hitInfo)
    {
        Vector3 v3RelativePoint = hitInfo.transform.InverseTransformPoint(hitInfo.point);
        Vector3 v3HitCube = hitInfo.transform.position;

        if (v3RelativePoint.y >= 0.99f)
        {
            GameObject newEnemy = null;
            newEnemy = Instantiate(SelectEnemy, hitInfo.point, Quaternion.identity);
            newEnemy.transform.localEulerAngles = new Vector3(newEnemy.transform.localEulerAngles.x, objectRotation, newEnemy.transform.localEulerAngles.z);
            newEnemy.transform.parent = EnemyParent.transform;
            switch (SelectEnemy.name)
            {
                case "Skeleton":
                    {
                        newEnemy.transform.position = new Vector3(newEnemy.transform.position.x, v3HitCube.y + 1f, newEnemy.transform.position.z);

                        break;
                    }

            }
        }
    }

    void CreatePortal(RaycastHit hitInfo)   // 포탈 설치
    {
        Vector3 v3RelativePoint = hitInfo.transform.InverseTransformPoint(hitInfo.point);
        Vector3 v3HitCube = hitInfo.transform.position;

        if (v3RelativePoint.y >= 0.99f)
        {
            GameObject newPortal = null;
            newPortal = Instantiate(SelectPortal, v3HitCube, Quaternion.identity);
            newPortal.transform.parent = PortalParent.transform;
            
            newPortal.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 1.5f, hitInfo.point.z);
            newPortal.transform.localEulerAngles = new Vector3(-90f, 0, 0);


            newPortal.AddComponent<BoxCollider>();
            newPortal.GetComponent<BoxCollider>().center = Vector3.zero;
            newPortal.GetComponent<BoxCollider>().size = new Vector3(1.2f, 1.2f, 1.2f);

            string filePath = "";

#if UNITY_EDITOR
            if(newPortal.name.Substring(0, 4).Equals("Blue") == false)    // 파란 포탈은 링크맵 ㄴㄴ
            {
                filePath = EditorUtility.OpenFilePanel("Link Map File Dialog"
                                           , Application.dataPath
                                           , "xml");
            }
#endif
            string linkMapName;
            
            if(newPortal.name.Substring(0, 4).Equals("Blue") == false)     // 파란 포탈은 링크맵 ㄴㄴ
            {
                linkMapName = filePath.Substring(filePath.LastIndexOf("/") + 1);             // 우측 상단에 현재 맵 xml 파일의 이름 출력 
                linkMapName = linkMapName.Substring(0, linkMapName.Length - 4);
                newPortal.GetComponent<PortalInfo>().LinkMapName = linkMapName;
            }

            newPortal.GetComponent<PortalInfo>().portalNumber = int.Parse(UIManager.Instance.input_PortalNumber.value);

            Debug.Log("Portal Link Succes!");
        }
    }

    void CreateNpc(RaycastHit hitInfo)  // Npc 설치
    {
        Vector3 v3RelativePoint = hitInfo.transform.InverseTransformPoint(hitInfo.point);
        Vector3 v3HitCube = hitInfo.transform.position;

        if (v3RelativePoint.y >= 0.99f)
        {
            GameObject newNpc = null;
            newNpc = Instantiate(SelectNpc, v3HitCube, Quaternion.identity);
            Destroy(newNpc.GetComponent<NpcDialog>());
            newNpc.transform.localEulerAngles = new Vector3(newNpc.transform.localEulerAngles.x, objectRotation, newNpc.transform.localEulerAngles.z);
            newNpc.transform.parent = NpcParent.transform;
            switch (SelectNpc.name)
            {
                case "Barbarian Wyder-Blue":
                    {
                        newNpc.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 1f, hitInfo.point.z);
                        break;
                    }
                case "Barbarian Wyder-Gold":
                    {
                        newNpc.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 1f, hitInfo.point.z);

                        break;
                    }
                case "Barbarian Wyder-Green":
                    {
                        newNpc.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 1f, hitInfo.point.z);
                        break;
                    }
                case "Barbarian Wyder-Red":
                    {
                        newNpc.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 1f, hitInfo.point.z);

                        break;
                    }

            }
        }
    }
}
