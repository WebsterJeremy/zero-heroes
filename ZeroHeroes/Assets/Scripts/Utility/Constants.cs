using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Utility
{
    public class Constants
    {
        public enum FSMActionState { NONE, ACTIVE, COMPLETED, TERMINATED }

        public static string GenerateUniqueId() {
            return Guid.NewGuid().ToString();
        }

        public class Actions
        {
            public static string PICKUP_ITEM = "Pickup {0}";
            public static string PICKUP_ITEMS = "Pickup {0} x{1}";
            public static string DROP_ITEM = "Drop {0}";
            public static string DROP_ITEMS = "Drop {0} x{1}";
        }
    }
}
