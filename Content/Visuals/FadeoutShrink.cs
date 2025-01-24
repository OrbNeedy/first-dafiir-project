using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathtoDarkSide.Content.Visuals
{
    public class FadeoutShrink : VFX
    {
        public override void Update(ref float texture, ref float positionX, ref float positionY,
            ref float rotation, ref float scaleX, ref float scaleY, ref float r, ref float g, ref float b,
            ref float a, ref float layer, ref float frame, ref float time, ref float ai1, ref float ai2)
        {
            if (scaleX > 0) scaleX -= 0.02f;
            if (scaleY > 0) scaleY -= 0.02f;
            a -= 0.02f;
        }
    }
}
