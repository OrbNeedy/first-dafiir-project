using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathtoDarkSide.Content.Visuals
{
    public class VFX
    {
        public virtual void Update(ref float texture, ref float positionX, ref float positionY, 
            ref float rotation, ref float scaleX, ref float scaleY, ref float r, ref float g, ref float b, 
            ref float a, ref float layer, ref float frame, ref float time, ref float ai1, ref float ai2)
        {
        }
    }
}
