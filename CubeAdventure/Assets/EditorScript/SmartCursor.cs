using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartCursor : MonoBehaviour {

    public  GameObject selectObject;
    GameObject cursorObject;

    void Start()
    {
        cursorObject = Instantiate(selectObject, Vector3.zero, Quaternion.identity);
        cursorObject.transform.eulerAngles = new Vector3(cursorObject.transform.localEulerAngles.x, this.GetComponent<MouseRay>().objectRotation, cursorObject.transform.localEulerAngles.z);
        Destroy(cursorObject.GetComponent<BoxCollider>());
        cursorObject.gameObject.SetActive(false);

    }

    void LateUpdate()
    {
        CursorTypeChange();
        CursorActive();
    }

    void CursorTypeChange()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectObject = this.GetComponent<MouseRay>().SelectCube;
            CursorChange();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectObject = this.GetComponent<MouseRay>().SelectNature;
            CursorChange();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectObject = this.GetComponent<MouseRay>().SelectBuild;
            CursorChange();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectObject = this.GetComponent<MouseRay>().SelectBridge;
            CursorChange();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            selectObject = this.GetComponent<MouseRay>().SelectEnemy;
            CursorChange();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            selectObject = this.GetComponent<MouseRay>().SelectPortal;
            CursorChange();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            selectObject = this.GetComponent<MouseRay>().SelectNpc;
            CursorChange();
        }

    }

    public void CursorChange()
    {
        Vector3 cursorPosition = cursorObject.transform.position;
        Destroy(cursorObject);
        cursorObject = null;
        cursorObject = Instantiate(selectObject, cursorPosition, Quaternion.identity);
        cursorObject.transform.eulerAngles = new Vector3(cursorObject.transform.localEulerAngles.x, this.GetComponent<MouseRay>().objectRotation, cursorObject.transform.localEulerAngles.z);
        cursorObject.gameObject.SetActive(false);


        switch (this.GetComponent<MouseRay>().setObjectKind)
        {
            case (int)ObjectKind.CUBE:
                {
                    Destroy(cursorObject.GetComponent<BoxCollider>());
                    break;
                }
            case (int)ObjectKind.NATURE:
                {
                    switch(selectObject.name)
                    {
                        case "ForestGrass01":
                            {
                                Destroy(cursorObject.GetComponent<BoxCollider>());
                                break;
                            }
                        case "ForestGrass02":
                            {
                                Destroy(cursorObject.GetComponent<BoxCollider>());
                                break;
                            }
                        case "DesertPear":
                            {
                                Destroy(cursorObject.GetComponent<BoxCollider>());
                                break;
                            }
                        default:
                            {
                                Destroy(cursorObject.GetComponent<CapsuleCollider>());
                                break;
                            }
                    }
                    break;
                }
            case (int)ObjectKind.BUILD:
                {
                    Destroy(cursorObject.GetComponent<BoxCollider>());
                    break;
                }
            case (int)ObjectKind.BRIDGE:
                {
                    Destroy(cursorObject.GetComponent<BoxCollider>());
                    break;
                }
            case (int)ObjectKind.NPC:
                {
                    Destroy(cursorObject.GetComponent<CapsuleCollider>());
                    Destroy(cursorObject.GetComponent<NpcDialog>());
                    break;
                }
            case (int)ObjectKind.ENEMY:
                {
                    Destroy(cursorObject.GetComponentInChildren<CapsuleCollider>());
                    Destroy(cursorObject.GetComponentInChildren<BoxCollider>());
                    break;
                }
        }
    }

    void CursorActive()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo))
        {
            if(hitInfo.transform.tag.Equals("Cube"))
            {
                if (!(cursorObject.gameObject.activeSelf))
                {
                    cursorObject.gameObject.SetActive(true);

                }
                
                if(Input.GetKey(KeyCode.C))    // c 클릭동안 커서 숨기기
                {
                    cursorObject.gameObject.SetActive(false);
                }
                else
                {
                    CursorPosition(hitInfo);
                }

            }
            else
            {
                cursorObject.gameObject.SetActive(false);
            }

        }
        else  // ray가 안부딪히면
        {
            cursorObject.gameObject.SetActive(false);
        }
    }
    
    void CursorPosition(RaycastHit hitInfo)
    {
        Vector3 v3RelativePoint = hitInfo.transform.InverseTransformPoint(hitInfo.point);
        Vector3 v3HitCube = hitInfo.transform.position;

        switch (this.GetComponent<MouseRay>().setObjectKind)
        {
            case (int)ObjectKind.CUBE:
                {
                    CubeCursorPosition(hitInfo, v3RelativePoint, v3HitCube);
                    break;
                }
            case (int)ObjectKind.NATURE:
                {
                    NatureCursorPosition(hitInfo, v3RelativePoint, v3HitCube);
                    break;
                }
            case (int)ObjectKind.BUILD:
                {
                    BuildCursorPosition(hitInfo, v3RelativePoint, v3HitCube);
                    break;
                }
            case (int)ObjectKind.BRIDGE:
                {
                    BridgeCursorPosition(hitInfo, v3RelativePoint, v3HitCube);
                    break;
                }
            case (int)ObjectKind.ENEMY:
                {
                    EnemyCursorPosition(hitInfo, v3RelativePoint, v3HitCube);
                    break;
                }
            case (int)ObjectKind.PORTAL:
                {
                    PortalCursorPosition(hitInfo, v3RelativePoint, v3HitCube);
                    break;
                }
            case (int)ObjectKind.NPC:
                {
                    NpcCursorPosition(hitInfo, v3RelativePoint, v3HitCube);
                    break;
                }
        }
    }

    void CubeCursorPosition(RaycastHit hitInfo, Vector3 v3RelativePoint, Vector3 v3HitCube)
    {

        cursorObject.transform.localEulerAngles = Vector3.zero;
        if (v3RelativePoint.x >= 0.99f)
        {
            cursorObject.transform.position = v3HitCube + (Vector3.right * 2);
        }
        else if (v3RelativePoint.x <= -0.99f)
        {
            cursorObject.transform.position = v3HitCube + (Vector3.left * 2);
        }
        else if (v3RelativePoint.y >= 0.99f)
        {
            cursorObject.transform.position = v3HitCube + (Vector3.up * 2);
        }
        else if (v3RelativePoint.y <= -0.99f)
        { 
            cursorObject.transform.position = v3HitCube + (Vector3.down * 2);
        }
        else if (v3RelativePoint.z >= 0.99f)
        {
            cursorObject.transform.position = v3HitCube + (Vector3.forward * 2);
        }
        else if (v3RelativePoint.z <= -0.99f)
        {
            cursorObject.transform.position = v3HitCube + (Vector3.back * 2);
        }
    }
    
    void NatureCursorPosition(RaycastHit hitInfo, Vector3 v3RelativePoint, Vector3 v3HitCube)
    {
        cursorObject.transform.localEulerAngles = new Vector3(0f, this.GetComponent<MouseRay>().objectRotation, 0f);
        if (v3RelativePoint.y >= 0.99f)
        {
            switch (selectObject.name)
            {
                case "ForestGrass01":
                    {
                        cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 1.2372f, hitInfo.point.z);
                        break;
                    }
                case "ForestGrass02":
                    {
                        cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 1.2372f, hitInfo.point.z);
                        break;
                    }
                case "ForestTreeAppleShort":
                    {
                        cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 0.9861372f, hitInfo.point.z);
                        break;
                    }
                case "ForestTreeAppleTall":
                    {
                        cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 0.9861372f, hitInfo.point.z);
                        break;
                    }
                case "ForestTreeDShort":
                    {
                        cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 0.9861372f, hitInfo.point.z);
                        break;
                    }
                case "ForestTreeDTall":
                    {
                        cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 0.9861372f, hitInfo.point.z);
                        break;
                    }
                case "ForestTreePineShort":
                    {
                        cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 2.054519f, hitInfo.point.z);
                        break;
                    }
                case "ForestTreePineShort2":
                    {
                        cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 3.109039f, hitInfo.point.z);
                        break;
                    }
                case "ForestTreePineTall":
                    {
                        cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 2.054519f, hitInfo.point.z);
                        break;
                    }
                case "ForestTreePineTall2":
                    {
                        cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 3.109039f, hitInfo.point.z);
                        break;
                    }
                case "DesertCactus01":
                    {
                        cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 1.3f, hitInfo.point.z);
                        break;
                    }
                case "DesertCactus02":
                    {
                        cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 1.3f, hitInfo.point.z);
                        break;
                    }
            }
        }
        else
        {
            cursorObject.gameObject.SetActive(false);
        }
    }

    void BuildCursorPosition(RaycastHit hitInfo, Vector3 v3RelativePoint, Vector3 v3HitCube)
    {
        cursorObject.transform.localEulerAngles = new Vector3(0f, this.GetComponent<MouseRay>().objectRotation, 0f);
        if (v3RelativePoint.y >= 0.99f)
        {
            switch (selectObject.name)
            {
                case "ForestBrazzierBlue":
                    {
                        cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 1.96f, hitInfo.point.z);
                        break;
                    }
                case "ForestCastle_Blue":
                    {
                        cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 1.009725f, hitInfo.point.z);
                        break;
                    }
                case "ForestTower_Blue":
                    {
                        cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 0.9760386f, hitInfo.point.z);
                        break;
                    }
                case "DesertBrazzier_Blue":
                    {
                        cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 2.304677f, hitInfo.point.z);
                        break;
                    }
                case "DesertCastle_Blue":
                    {
                        cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 1.009725f, hitInfo.point.z);
                        break;
                    }
                case "DesertFlag_Blue":
                    {
                        cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 1f, hitInfo.point.z);
                        break;
                    }
                case "DesertTower_Blue":
                    {
                        cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 4.095812f, hitInfo.point.z);

                        break;
                    }

            }
        }
        else
        {
            cursorObject.gameObject.SetActive(false);
        }
    }

    void BridgeCursorPosition(RaycastHit hitInfo, Vector3 v3RelativePoint, Vector3 v3HitCube)
    {
        cursorObject.transform.localEulerAngles = new Vector3(0f, this.GetComponent<MouseRay>().objectRotation, 0f);
        if (v3RelativePoint.y >= 0.99f)
        {
            switch (selectObject.name)
            {
                case "DesertBridgeEnd":
                    {
                        cursorObject.transform.position = new Vector3(v3HitCube.x, v3HitCube.y + 1.40656f, v3HitCube.z);
                        break;
                    }
                case "DesertBridgeMainW":
                    {
                        cursorObject.transform.position = new Vector3(v3HitCube.x, v3HitCube.y + 1.419906f, v3HitCube.z);

                        break;
                    }
                case "DesertBridgeExtra01":
                    {
                        cursorObject.transform.position = new Vector3(v3HitCube.x, v3HitCube.y + 1.419906f, v3HitCube.z);
                        break;
                    }
                case "DesertBridgeExtra01W":
                    {
                        cursorObject.transform.position = new Vector3(v3HitCube.x, v3HitCube.y + 1.419906f, v3HitCube.z);
                        break;
                    }
                case "DesertBridgeEndW":
                    {
                        cursorObject.transform.position = new Vector3(v3HitCube.x, v3HitCube.y + 1.40656f, v3HitCube.z);
                        break;
                    }
                case "DesertBridgeMain":
                    {
                        cursorObject.transform.position = new Vector3(v3HitCube.x, v3HitCube.y + 1.419906f, v3HitCube.z);
                        break;
                    }
                case "ForestBridgeRegular":
                    {
                        cursorObject.transform.position = new Vector3(v3HitCube.x, v3HitCube.y + 1.1666f, v3HitCube.z);
                        break;
                    }
                case "ForestBridgeWide":
                    {
                        cursorObject.transform.position = new Vector3(v3HitCube.x, v3HitCube.y + 1.1666f, v3HitCube.z);
                        break;
                    }

            }
        }
        else
        {
            cursorObject.gameObject.SetActive(false);
        }
    }
    void EnemyCursorPosition(RaycastHit hitInfo, Vector3 v3RelativePoint, Vector3 v3HitCube)
    {
        cursorObject.transform.localEulerAngles = new Vector3(0f, this.GetComponent<MouseRay>().objectRotation, 0f);
        if (v3RelativePoint.y >= 0.99f)
        {
            switch (selectObject.name)
            {
                case "Skeleton":
                    {
                        cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 1f, hitInfo.point.z);

                        break;
                    }
            }
        }
        else
        {
            cursorObject.gameObject.SetActive(false);
        }
    }


    void PortalCursorPosition(RaycastHit hitInfo, Vector3 v3RelativePoint, Vector3 v3HitCube)
    {
        cursorObject.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
        if (v3RelativePoint.y >= 0.99f)
        {
            cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 1.5f, hitInfo.point.z);
            
        }
        else
        {
            cursorObject.gameObject.SetActive(false);
        }
    }

    void NpcCursorPosition(RaycastHit hitInfo, Vector3 v3RelativePoint, Vector3 v3HitCube)
    {
        cursorObject.transform.localEulerAngles = new Vector3(0f, this.GetComponent<MouseRay>().objectRotation, 0f);
        if (v3RelativePoint.y >= 0.99f)
        {
            cursorObject.transform.position = new Vector3(hitInfo.point.x, v3HitCube.y + 1f, hitInfo.point.z);

         
        }
        else
        {
            cursorObject.gameObject.SetActive(false);
        }
    }
}
