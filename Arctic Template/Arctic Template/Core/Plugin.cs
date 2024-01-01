using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using System.Reflection;

namespace Template.Menu
{
    [BepInPlugin(guid, title, version)]
    public class Plugin : BaseUnityPlugin
    {
        public const string guid = "org.rxin.arctictemplate";
        public const string title = "Arctic Template";
        public const string version = "2.0.0";

        public void Start()
        {
            var harmony = new Harmony(guid);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
