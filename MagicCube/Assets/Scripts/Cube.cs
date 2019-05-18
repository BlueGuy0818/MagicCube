using UnityEngine;

public class Cube : MonoBehaviour
{
    struct RotateInfo
    {
        public string axis;
        public float dir;
        public float a;
    };

    const int TRANS = 2;
    public Camera camera;
    Vector3 offset;
    public GameObject cubePrefab;
    public GameObject cubeParent;
    GameObject[] cubes = new GameObject[27];
    Transform[] matx = new Transform[27];
    Vector3 vecs;
    Vector3 vece;
    GameObject pickedNodes;
    GameObject pickedNodee;
    Vector3 vecms;
    Vector3 vecme;
    bool done = false;

    void Start()
    {
        for (int i = 0; i < 27; i++)
        {
            cubes[i] = Instantiate(cubePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            cubes[i].transform.parent = cubeParent.transform;
        }

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    int number = 9 * (i + 1) + 3 * (j + 1) + (k + 2);
                    GameObject cube = cubes[number-1];
                    cube.name = "Cube" + number;
                    cube.transform.Translate(new Vector3(i * TRANS, j * TRANS, k * TRANS));
                    matx[number - 1] = cube.transform;
                }
            }
        }

        offset = camera.transform.position - cubeParent.transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pickedNodes = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                pickedNodes = hit.collider.gameObject;
                vecs = hit.point;
            }

            if (pickedNodes != null)
            {
                for (int i = 0; i < 27; i++)
                {
                    if (pickedNodes.name == cubes[i].name)
                    {
                        vecms = matx[i].transform.position;
                        Debug.Log(pickedNodes.name);
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            done = false;
        }

        if (Input.GetMouseButton(0))
        {
            if (!done)
            {
                pickedNodee = null;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    pickedNodee = hit.collider.gameObject;
                    vece = hit.point;
                }

                if (pickedNodes != null)
                {
                    if (pickedNodee != null)
                    {
                        for (int i = 0; i < 27; i++)
                        {
                            if (pickedNodee.name == cubes[i].name)
                            {
                                vecme = matx[i].transform.position;
                            }
                        }
                    }

                    RotateInfo rotateInfo = rotationinfo();
                    rotate(rotateInfo);

                    if (rotateInfo.axis != "")
                    {
                        done = true;
                    }
                }
            }
        }

        Rotate();
        Scale();
    }

    RotateInfo rotationinfo()
    {
        RotateInfo rotateInfo = new RotateInfo();
        string axis = "";
        float dir = 0.0f;
        float a = 0.0f;

        if ((Mathf.Abs(vecme.x - vecms.x) >= 0.01) || (Mathf.Abs(vecme.y - vecms.y) >= 0.01) || (Mathf.Abs(vecme.z - vecms.z) >= 0.01))
        {
            if ((Mathf.Abs(vecme.x - vecms.x) < 0.01) && (Mathf.Abs(vecme.y - vecms.y) < 0.01))
            {
                if ((Mathf.Abs(Mathf.Abs(vece.x) - 3.0f) < 0.01) && (Mathf.Abs(Mathf.Abs(vecs.x) - 3.0f) < 0.01))
                {
                    axis = "y";
                    a = vecme.y / 100;
                    float temp = (vece.z - vecs.z) * vece.x;
                    dir = (-1) * temp / Mathf.Abs(temp);
                }
                else if ((Mathf.Abs(Mathf.Abs(vece.y) - 3.0f) < 0.01) && (Mathf.Abs(Mathf.Abs(vecs.y) - 3.0f) < 0.01))
                {
                    axis = "x";
                    a = vecme.x / 100;
                    float temp = (vece.z - vecs.z) * vece.y;
                    dir = temp / Mathf.Abs(temp);
                }
            }
            else if ((Mathf.Abs(vecme.x - vecms.x) < 0.01) && (Mathf.Abs(vecme.z - vecms.z) < 0.01))
            {
                if ((Mathf.Abs(Mathf.Abs(vece.x) - 3.0f) < 0.01) && (Mathf.Abs(Mathf.Abs(vecs.x) - 3.0f) < 0.01))
                {
                    axis = "z";
                    a = vecme.z / 100;
                    float temp = (vece.y - vecs.y) * vece.x;
                    dir = temp / Mathf.Abs(temp);
                }
                else if ((Mathf.Abs(Mathf.Abs(vece.z) - 3.0f) < 0.01) && (Mathf.Abs(Mathf.Abs(vecs.z) - 3.0f) < 0.01))
                {
                    axis = "x";
                    a = vecme.x / 100;
                    float temp = (vece.y - vecs.y) * vece.z;
                    dir = (-1) * temp / Mathf.Abs(temp);
                }
            }
            else if ((Mathf.Abs(vecme.y - vecms.y) < 0.01) && (Mathf.Abs(vecme.z - vecms.z) < 0.01))
            {
                if ((Mathf.Abs(Mathf.Abs(vece.y) - 3.0f) < 0.01) && (Mathf.Abs(Mathf.Abs(vecs.y) - 3.0f) < 0.01))
                {
                    axis = "z";
                    a = vecme.z / 100;
                    float temp = (vece.x - vecs.x) * vece.y;
                    dir = (-1) * temp / Mathf.Abs(temp);
                }
                else if ((Mathf.Abs(Mathf.Abs(vece.z) - 3.0f) < 0.01) && (Mathf.Abs(Mathf.Abs(vecs.z) - 3.0f) < 0.01))
                {
                    axis = "y";
                    a = vecme.y / 100;
                    float temp = (vece.x - vecs.x) * vece.z;
                    dir = temp / Mathf.Abs(temp);
                }
            }
        }
        else if ((Mathf.Abs(vecme.x - vecms.x) < 0.01) && (Mathf.Abs(vecme.y - vecms.y) < 0.01) && (Mathf.Abs(vecme.z - vecms.z) < 0.01))
        {
            if (((Mathf.Abs(Mathf.Abs(vecs.x) - 3.0f) < 0.01) && (Mathf.Abs(Mathf.Abs(vece.y) - 3.0f) < 0.01)) ||
                ((Mathf.Abs(Mathf.Abs(vecs.y) - 3.0f) < 0.01) && (Mathf.Abs(Mathf.Abs(vece.x) - 3.0f) < 0.01)))
            {
                if ((Mathf.Abs(vece.y - vecs.y) > 0.1) && (Mathf.Abs(vece.x - vecs.x) > 0.1))
                {
                    axis = "z";
                    a = vecme.z / 100;
                    float temp = vecs.x * vece.y * (Mathf.Abs(vece.y) - Mathf.Abs(vecs.y));
                    dir = temp / Mathf.Abs(temp);
                }
            }
            else if (((Mathf.Abs(Mathf.Abs(vecs.x) - 3.0f) < 0.01) && (Mathf.Abs(Mathf.Abs(vece.z) - 3.0f) < 0.01)) ||
                ((Mathf.Abs(Mathf.Abs(vecs.z) - 3.0f) < 0.01) && (Mathf.Abs(Mathf.Abs(vece.x) - 3.0f) < 0.01)))
            {
                if ((Mathf.Abs(vece.z - vecs.z) > 0.1) && (Mathf.Abs(vece.x - vecs.x) > 0.1))
                {
                    axis = "y";
                    a = vecme.y / 100;
                    float temp = vecs.z * vece.x * (Mathf.Abs(vece.x) - Mathf.Abs(vecs.x));
                    dir = temp / Mathf.Abs(temp);
                }
            }
            else if (((Mathf.Abs(Mathf.Abs(vecs.z) - 3.0f) < 0.01) && (Mathf.Abs(Mathf.Abs(vece.y) - 3.0f) < 0.01)) ||
                ((Mathf.Abs(Mathf.Abs(vecs.y) - 3.0f) < 0.01) && (Mathf.Abs(Mathf.Abs(vece.z) - 3.0f) < 0.01)))
            {
                if ((Mathf.Abs(vece.z - vecs.z) > 0.1) && (Mathf.Abs(vece.y - vecs.y) > 0.1))
                {
                    axis = "x";
                    a = vecme.x / 100;
                    float temp = vecs.y * vece.z * (Mathf.Abs(vece.z) - Mathf.Abs(vecs.z));
                    dir = temp / Mathf.Abs(temp);
                }
            }
        }

        if (pickedNodee == null)
        {
            if (Mathf.Abs(Mathf.Abs(vecs.x) - 3.0f) < 0.01)
            {
                axis = (Mathf.Abs(vece.y) >= Mathf.Abs(vece.z)) ? "z" : "y";
                string tempos = Mathf.Abs(vece.y) >= Mathf.Abs(vece.z) ? "y" : "z";

                if (axis == "z")
                {
                    a = vecms.z / 100;
                }
                else
                {
                    a = vecms.y / 100;
                }

                float temp;

                if (axis == "z")
                {
                    if (tempos == "y")
                    {
                        temp = vecs.x * vece.y;
                    }
                    else
                    {
                        temp = vecs.x * vece.z;
                    }
                }
                else
                {
                    if (tempos == "y")
                    {
                        temp = (-1) * vecs.z * vece.y;
                    }
                    else
                    {
                        temp = (-1) * vecs.z * vece.z;
                    }
                }

                dir = temp / Mathf.Abs(temp);
            }
            else if (Mathf.Abs(Mathf.Abs(vecs.y) - 3.0f) < 0.01)
            {
                axis = (Mathf.Abs(vece.x) >= Mathf.Abs(vece.z)) ? "z" : "x";
                string tempos = Mathf.Abs(vece.x) >= Mathf.Abs(vece.z) ? "x" : "z";

                if (axis == "z")
                {
                    a = vecms.z / 100;
                }
                else
                {
                    a = vecms.x / 100;
                }

                float temp;

                if (axis == "x")
                {
                    if (tempos == "x")
                    {
                        temp = vecs.y * vece.x;
                    }
                    else
                    {
                        temp = vecs.y * vece.z;
                    }
                }
                else
                {
                    if (tempos == "x")
                    {
                        temp = (-1) * vecs.y * vece.x;
                    }
                    else
                    {
                        temp = (-1) * vecs.y * vece.z;
                    }
                }

                dir = temp / Mathf.Abs(temp);
            }
            else if (Mathf.Abs(Mathf.Abs(vecs.z) - 3.0f) < 0.01)
            {
                axis = (Mathf.Abs(vece.y) >= Mathf.Abs(vece.x)) ? "x" : "y";
                string tempos = Mathf.Abs(vece.y) >= Mathf.Abs(vece.x) ? "y" : "x";

                if (axis == "x")
                {
                    a = vecms.x / 100;
                }
                else
                {
                    a = vecms.y / 100;
                }

                float temp;

                if (axis == "y")
                {
                    if (tempos == "x")
                    {
                        temp = vecs.z * vece.x;
                    }
                    else
                    {
                        temp = vecs.z * vece.y;
                    }
                }
                else
                {
                    if (tempos == "x")
                    {
                        temp = (-1) * vecs.z * vece.x;
                    }
                    else
                    {
                        temp = (-1) * vecs.z * vece.y;
                    }
                }

                dir = temp / Mathf.Abs(temp);
            }
        }

        rotateInfo.axis = axis;
        rotateInfo.dir = dir;
        rotateInfo.a = a;

        return rotateInfo;
    }

    void rotate(RotateInfo rotateInfo)
    {
        if (rotateInfo.axis != "")
        {
            if (rotateInfo.axis == "x")
            {
                for (int i = 0; i < 27; i++)
                {
                    if (Mathf.Abs(matx[i].transform.localPosition.x - 100 * rotateInfo.a) < 1)
                    {
                        matx[i].RotateAround(cubeParent.transform.localPosition, Vector3.right * rotateInfo.dir, 90);
                    }
                }
            }
            else if (rotateInfo.axis == "y")
            {
                for (int i = 0; i < 27; i++)
                {
                    if (Mathf.Abs(matx[i].transform.localPosition.y - 100 * rotateInfo.a) < 1)
                    {
                        matx[i].RotateAround(cubeParent.transform.localPosition, Vector3.up * rotateInfo.dir, 90);
                    }
                }
            }
            else if (rotateInfo.axis == "z")
            {
                for (int i = 0; i < 27; i++)
                {
                    if (Mathf.Abs(matx[i].transform.localPosition.z - 100 * rotateInfo.a) < 1)
                    {
                        matx[i].RotateAround(cubeParent.transform.localPosition, Vector3.forward * rotateInfo.dir, 90);
                    }
                }
            }
        }
    }

    private void Scale()
    {
        float dis = offset.magnitude;
        dis += Input.GetAxis("Mouse ScrollWheel") * (-5);

        if (dis < 10 || dis > 40)
        {
            return;
        }

        offset = offset.normalized * dis;
        camera.transform.position = cubeParent.transform.position + offset;
    }

    private void Rotate()
    {
        if (Input.GetMouseButton(1))
        {
            Vector3 pos = camera.transform.position;
            Vector3 rot = camera.transform.eulerAngles;

            camera.transform.RotateAround(cubeParent.transform.position, Vector3.up, Input.GetAxis("Mouse X") * 10);
            camera.transform.RotateAround(cubeParent.transform.position, Vector3.left, Input.GetAxis("Mouse Y") * 10);
            float x = camera.transform.eulerAngles.x;
            float y = camera.transform.eulerAngles.y;
            
            offset = camera.transform.position - cubeParent.transform.position;
            camera.transform.position = cubeParent.transform.position + offset;
        }
    }
}