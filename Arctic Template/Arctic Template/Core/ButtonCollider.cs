using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Template.Menu
{
    public class ButtonCollider : MonoBehaviour
    {
        public string btnName;

        private void OnTriggerEnter(Collider collider)
        {
            if (Time.frameCount >= ArcticTemplate.framePressCooldown + 30)
            {
                ArcticTemplate.ToggleButton(btnName);
                ArcticTemplate.framePressCooldown = Time.frameCount;
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, false, 0.25f);
                GorillaTagger.Instance.StartVibration(false, 2f, 0.3f);
            }
        }
    }
}
