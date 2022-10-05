using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TrainerTyrant
{
    /**
     * <summary>A complete representation of every pokemon's learnset.</summary>
     */
    public class LearnsetSet
    {
        private Dictionary<string, List<LevelUpMove>> _data;
        private bool _initialized;
        public bool Initialized { get { return _initialized; } }

        public LearnsetSet()
        {
            _data = null;
            _initialized = false;
        }

        public void InitializeWithJSON(string JSON)
        {
            //Don't initialize if already initialized.
            if (Initialized)
                return;
        }

    }

    /**
     * <summary>The level and move learned at a specific level</summary>
     */
    class LevelUpMove
    {
        public int Level { get; set; }
        public string Move { get; set; }
    }
}
