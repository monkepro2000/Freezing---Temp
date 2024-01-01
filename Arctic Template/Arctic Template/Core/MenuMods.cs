using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using BepInEx;
using HarmonyLib;
using UnityEngine.UIElements;

namespace Template.Menu
{
    public class MenuMods : MonoBehaviour
    {
        public static Material PlatColor = new Material(Shader.Find("GorillaTag/UberShader"));

        public static bool RightToggle;

        public static bool LeftToggle;

        public static GameObject rPlat;

        public static GameObject lPlat;

        public static Vector3 scale = new Vector3(0.0100f, 0.23f, 0.3625f);

        public static void Platforms()
        {
            var gripdownright = ControllerInputPoller.instance.rightControllerGripFloat;
            var gripdownleft = ControllerInputPoller.instance.leftControllerGripFloat;
            if (gripdownright == 1f && RightToggle)
            {
                rPlat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                PlatColor.color = Color.black;
                rPlat.GetComponent<Renderer>().material = PlatColor;
                rPlat.transform.localScale = scale;
                rPlat.transform.position = new Vector3(0f, -0.00825f, 0f) + GorillaLocomotion.Player.Instance.rightControllerTransform.position;
                rPlat.transform.rotation = GorillaLocomotion.Player.Instance.rightControllerTransform.rotation;
                RightToggle = false;
            }
            if (gripdownright != 1f)
            {
                GameObject.Destroy(rPlat);
                RightToggle = true;
            }
            if (gripdownleft == 1f && LeftToggle)
            {
                lPlat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                PlatColor.color = Color.black;
                lPlat.GetComponent<Renderer>().material = PlatColor;
                lPlat.transform.localScale = scale;
                lPlat.transform.position = new Vector3(0f, -0.00825f, 0f) + GorillaLocomotion.Player.Instance.leftControllerTransform.position;
                lPlat.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.rotation;
                LeftToggle = false;
            }
            if (gripdownleft != 1f)
            {
                GameObject.Destroy(lPlat);
                LeftToggle = true;
            }
        }

        public static void Fly()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * 15f; // Makes your rig go forward
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero; // makes it to where your rig won't fall while flying.
            }
        }

        public static void Noclip()
        {
            if (ControllerInputPoller.TriggerFloat(XRNode.RightHand) == 1f)
            {
                foreach (MeshCollider m in Resources.FindObjectsOfTypeAll<MeshCollider>())
                {
                    m.enabled = false;
                }
            }
            else
            {
                foreach (MeshCollider m2 in Resources.FindObjectsOfTypeAll<MeshCollider>())
                {
                    m2.enabled = true;
                }
            }
        }

        public static void Chams()
        {
            Material lavaMat = new Material(Shader.Find("GUI/Text Shader"));
            Material normalMat = new Material(Shader.Find("GUI/Text Shader"));
            lavaMat.color = Color.red;
            normalMat.color = Color.green;
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (!vrrig.isOfflineVRRig && !vrrig.isMyPlayer)
                {
                    if (vrrig.mainSkin.material.name.Contains("fected"))
                    {
                        vrrig.mainSkin.material = lavaMat;
                    }
                    else
                    {
                        vrrig.mainSkin.material = normalMat;
                    }
                }
            }
        }
    }
}
