using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using HarmonyLib;
using BepInEx;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using PlayFab.GroupsModels;

namespace Template.Menu
{
    [HarmonyPatch(typeof(GorillaLocomotion.Player))]
    [HarmonyPatch("LateUpdate", MethodType.Normal)]
    public class ArcticTemplate : MonoBehaviour
    {
        public static GameObject MenuObj;

        public static GameObject canvasObj = null;

        public static GameObject reference = null;

        public static int framePressCooldown = 0;

        static int btnCooldown;

        public static int pageButtons = 6;

        public static int pageNumber = 0;

        public static string[] buttons = new string[]
        {
            "Placeholder" +
            "",
            "",
            "PlaceHolder",
            "PlaceHolder",
            "PlaceHolder",
            "PlaceHolder",
            "PlaceHolder",
        };

        public static bool?[] enabled = new bool?[]
        {
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
        };
        public static void Prefix()
        {
            try
            {
                if (ControllerInputPoller.instance.leftControllerSecondaryButton && MenuObj == null)
                {
                    DrawFrame();
                    if (reference == null)
                    {
                        reference = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        reference.transform.parent = GorillaLocomotion.Player.Instance.rightControllerTransform;
                        if (ShowPointer)
                        {
                            reference.GetComponent<Renderer>().material = MenuPointer;
                        }
                        else
                        {
                            GameObject.Destroy(reference.GetComponent<MeshRenderer>());
                        }
                        reference.transform.localPosition = new Vector3(0f, -0.1f, 0f);
                        reference.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                    }
                }
                else if (!ControllerInputPoller.instance.leftControllerSecondaryButton && MenuObj != null)
                {
                    GameObject.Destroy(MenuObj);
                    MenuObj = null;
                    GameObject.Destroy(reference);
                    reference = null;
                }
                if (ControllerInputPoller.instance.leftControllerSecondaryButton && MenuObj != null)
                {
                    MenuObj.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.transform.position;
                    MenuObj.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.transform.rotation;
                }
                if (btnCooldown > 0)
                {
                    if (Time.frameCount > btnCooldown)
                    {
                        btnCooldown = 0;
                        GameObject.Destroy(MenuObj);
                        MenuObj = null;
                        DrawFrame();
                    }
                }

                if (enabled[0] == true)
                {
                    PhotonNetwork.Disconnect();
                }

                if (enabled[1] == true)
                {
                    GorillaLocomotion.Player.Instance.maxJumpSpeed = 8.5f;
                }
                else
                {
                    GorillaLocomotion.Player.Instance.maxJumpSpeed = 6.5f;
                }


                UpdateMaterialColors();
                if (isColorChangerActive)
                {
                    UpdateColorChanger();
                }
            }
            catch
            {

            }
        }

        // Frame
        public static Material MenuColor = new Material(Shader.Find("GorillaTag/UberShader"));

        // Buttons
        public static Material BtnDisabledColor = new Material(Shader.Find("GorillaTag/UberShader"));
        public static Material BtnEnabledColor = new Material(Shader.Find("GorillaTag/UberShader"));

        // Page
        public static Material Next = new Material(Shader.Find("GorillaTag/UberShader"));
        public static Material Previous = new Material(Shader.Find("GorillaTag/UberShader"));

        // Menu Pointer
        public static Material MenuPointer = new Material(Shader.Find("GorillaTag/UberShader"));

        public static Material ColorChanger = new Material(Shader.Find("GorillaTag/UberShader"));

        public static bool ShowPointer = false;

        public static bool isColorChangerActive = false;

        public static void UpdateColorChanger()
        {
            ColorChanger.color = Color.Lerp(Color.blue, Color.blue, Mathf.PingPong(Time.time, 1f));
        }

        public static void UpdateMaterialColors()
        {
            MenuColor.color = Color.black; // Frame Color
            BtnDisabledColor.color = Color.red; // Disabled button color
            BtnEnabledColor.color = Color.green; // Enabled button color
            Next.color = Color.grey; // Nextpage button color
            Previous.color = Color.grey; // PreviousPage button color
            MenuPointer.color = Color.white;
            ShowPointer = false;
            // All materials are already set.
        }

        public static void DrawFrame()
        {
            // Holder
            MenuObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            MenuObj.transform.localScale = new Vector3(0.1f, 0.26f, 0.4f);
            GameObject.Destroy(MenuObj.GetComponent<Rigidbody>());
            GameObject.Destroy(MenuObj.GetComponent<Collider>());
            GameObject.Destroy(MenuObj.GetComponent<Renderer>());

            // MainFrame
            GameObject frame = GameObject.CreatePrimitive(PrimitiveType.Cube);
            frame.transform.parent = MenuObj.transform;
            frame.transform.rotation = Quaternion.identity;
            frame.transform.localScale = new Vector3(0.1f, 1f, 0.86f);
            frame.transform.position = new Vector3(.05f, 0, 0);
            GameObject.Destroy(frame.GetComponent<Rigidbody>());
            GameObject.Destroy(frame.GetComponent<BoxCollider>());
            frame.GetComponent<Renderer>().material = MenuColor;

            // Title Creation
            canvasObj = new GameObject();
            canvasObj.transform.parent = MenuObj.transform;
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            CanvasScaler canvasScale = canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvasScale.dynamicPixelsPerUnit = 1000;
            GameObject titleObj = new GameObject();
            titleObj.transform.parent = canvasObj.transform;
            Text title = titleObj.AddComponent<Text>();
            title.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            title.fontSize = 1;
            title.alignment = TextAnchor.MiddleCenter;
            title.resizeTextForBestFit = true;
            title.resizeTextMinSize = 0;
            title.text = "Arctic Template";
            RectTransform titleTransform = title.GetComponent<RectTransform>();
            titleTransform.localPosition = Vector3.zero;
            titleTransform.sizeDelta = new Vector2(.24f, .05f);
            titleTransform.position = new Vector3(.06f, 0f, .142f);
            titleTransform.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

            //Adding page buttons and spacing out the normal buttons
            AddPageButtons();
            string[] array2 = buttons.Skip(pageNumber * pageButtons).Take(pageButtons).ToArray();
            for (int i = 0; i < array2.Length; i++)
            {
                AddButton((float)i * 0.09f + 0.21f, array2[i]);
            }
        }

        public static void AddPageButtons()
        {
            int num = (buttons.Length + pageButtons - 1) / pageButtons;
            int num2 = pageNumber + 1; // Increments The Page
            int num3 = pageNumber - 1; // Decreases The Page
            if (num2 > num - 1)
            {
                num2 = 0;
            }
            if (num3 < 0)
            {
                num3 = num - 1;
            }
            float num4 = 0f;
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.transform.parent = MenuObj.transform;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(0.09f, 0.43f, 0.10f);
            gameObject.transform.localPosition = new Vector3(0.56f, 0.23f, -0.34f - num4);
            gameObject.AddComponent<ButtonCollider>().btnName = "PreviousPage";
            gameObject.GetComponent<Renderer>().material = Previous;
            GameObject gameObject2 = new GameObject();
            gameObject2.transform.parent = canvasObj.transform;
            Text text = gameObject2.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            text.text = "Previous";
            text.fontSize = 1;
            text.alignment = TextAnchor.MiddleCenter;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 0;
            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(0.08f, 0.03f);
            component.localPosition = new Vector3(0.064f, 0.07f, -0.13f);
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            num4 = 0.13f;
            GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(gameObject3.GetComponent<Rigidbody>());
            gameObject3.GetComponent<BoxCollider>().isTrigger = true;
            gameObject3.transform.parent = MenuObj.transform;
            gameObject3.transform.rotation = Quaternion.identity;
            gameObject3.transform.localScale = new Vector3(0.09f, 0.43f, 0.10f);
            gameObject3.transform.localPosition = new Vector3(0.56f, -0.23f, -0.21f - num4);
            gameObject3.AddComponent<ButtonCollider>().btnName = "NextPage";
            gameObject3.GetComponent<Renderer>().material = Next;
            GameObject gameObject4 = new GameObject();
            gameObject4.transform.parent = canvasObj.transform;
            Text text2 = gameObject4.AddComponent<Text>();
            text2.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            text2.text = "Next";
            text2.fontSize = 1;
            text2.alignment = TextAnchor.MiddleCenter;
            text2.resizeTextForBestFit = true;
            text2.resizeTextMinSize = 0;
            RectTransform component2 = text2.GetComponent<RectTransform>();
            component2.localPosition = Vector3.zero;
            component2.sizeDelta = new Vector2(0.08f, 0.03f);
            component2.localPosition = new Vector3(0.064f, -0.07f, -0.13f);
            component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        }

        public static void ToggleButton(string btnName)
        {
            int num = (buttons.Length + pageButtons - 1) / pageButtons;
            if (btnName == "NextPage")
            {
                if (pageNumber < num - 1)
                {
                    pageNumber++;
                }
                else
                {
                    pageNumber = 0;
                }
                UnityEngine.Object.Destroy(MenuObj);
                MenuObj = null;
                DrawFrame();
                return;
            }
            if (btnName == "PreviousPage")
            {
                if (pageNumber > 0)
                {
                    pageNumber--;
                }
                else
                {
                    pageNumber = num - 1;
                }
                UnityEngine.Object.Destroy(MenuObj);
                MenuObj = null;
                DrawFrame();
                return;
            }
            int num2 = -1;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (btnName == buttons[i])
                {
                    num2 = i;
                    break;
                }
            }
            if (enabled[num2].HasValue)
            {
                enabled[num2] = !enabled[num2];
                UnityEngine.Object.Destroy(MenuObj);
                MenuObj = null;
                DrawFrame();
            }
        }

        public static void AddButton(float offset, string text)
        {
            GameObject button = GameObject.CreatePrimitive(PrimitiveType.Cube);
            button.transform.parent = MenuObj.transform;
            button.transform.rotation = Quaternion.identity;
            button.transform.localScale = new Vector3(0.09f, 0.92f, 0.08f);
            button.transform.localPosition = new Vector3(0.56f, 0, 0.44f - offset);
            GameObject.Destroy(button.GetComponent<Rigidbody>());
            button.GetComponent<BoxCollider>().isTrigger = true;
            button.AddComponent<ButtonCollider>().btnName = text;

            int num = -1;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (text == buttons[i])
                {
                    num = i;
                    break;
                }
            }
            if (enabled[num] == false)
            {
                button.GetComponent<Renderer>().material = BtnDisabledColor;
            }
            else if (enabled[num] == true)
            {
                button.GetComponent<Renderer>().material = BtnEnabledColor;
            }
            else
            {
                button.GetComponent<Renderer>().material = BtnDisabledColor;
            }

            GameObject titleObj = new GameObject();
            titleObj.transform.parent = canvasObj.transform;
            Text title = titleObj.AddComponent<Text>();
            title.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            title.fontSize = 1;
            title.alignment = TextAnchor.MiddleCenter;
            title.resizeTextForBestFit = true;
            title.resizeTextMinSize = 0;
            title.text = text;
            RectTransform titleTransform = title.GetComponent<RectTransform>();
            titleTransform.localPosition = Vector3.zero;
            titleTransform.sizeDelta = new Vector2(.2f, .03f);
            titleTransform.position = new Vector3(.064f, 0f, .173f - (offset / 2.55f));
            titleTransform.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        }
    }
}
