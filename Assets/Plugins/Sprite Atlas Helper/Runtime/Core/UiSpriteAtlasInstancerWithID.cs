using UnityEngine;
using UnityEngine.UI;

namespace STVR.SAH
{
    [RequireComponent(typeof(Image))]
    public class UiSpriteAtlasInstancerWithID : UiSpriteAtlasInstancer
    {
        [SerializeField] int instancerId;

        public int InstancerId => instancerId;
    }

}
