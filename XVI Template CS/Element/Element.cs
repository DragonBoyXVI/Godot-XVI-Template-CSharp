using System;
using System.Collections.Generic;
using Godot;

namespace DragonXVI.Elements
{
    /// <summary>
    /// A Pokemon esque table for my elements.
    /// </summary>
    [GlobalClass, Tool]
    public partial class Element : Resource
    {
        /// <summary>
        /// Bit flags for element masks.
        /// </summary>
        [Flags]
        public enum Bit
        {
            None = 0,

            // Heat and fire, mostly heat
            Fire = 1 << 0,
            // Cold and ice, anything chilled.
            Ice = 1 << 1,
            // Toxins and waste, not just organic venom.
            Venom = 1 << 2,
            // Plants and nature
            Life = 1 << 3,
            // Metal and electricity, machinery
            Elec = 1 << 4,

            All = Fire | Ice | Venom | Life | Elec,
        }
        /// <summary>
        /// Type effectiveness enum.
        /// </summary>
        public enum Effectiveness
        {
            Weak = -1,
            Neutral = 0,
            Strong = 1,
        }

        public Element() { }
        public Element(int mask)
        {
            ElementValue = mask;
        }
        public Element(Bit mask)
        {
            ElementValue = (int)mask;
        }

        public static Element FromMask(int mask)
        {
            return new(mask);
        }

        /// <summary>
        /// How many elements there are.
        /// </summary>
        private const int ElementCount = 5;

        #region Tables

        private static readonly Dictionary<Bit, Effectiveness> TableFire = new()
        {
            { Bit.Fire, Effectiveness.Weak },
            { Bit.Ice, Effectiveness.Strong },
            { Bit.Venom, Effectiveness.Strong },
            { Bit.Life, Effectiveness.Strong },
            { Bit.Elec, Effectiveness.Weak },
        };
        private static readonly Dictionary<Bit, Effectiveness> TableIce = new()
        {
            { Bit.Fire, Effectiveness.Strong },
            { Bit.Ice, Effectiveness.Weak },
            { Bit.Venom, Effectiveness.Neutral },
            { Bit.Life, Effectiveness.Neutral },
            { Bit.Elec, Effectiveness.Neutral },
        };
        private static readonly Dictionary<Bit, Effectiveness> TableVenom = new()
        {
            { Bit.Fire, Effectiveness.Weak },
            { Bit.Ice, Effectiveness.Strong },
            { Bit.Venom, Effectiveness.Neutral },
            { Bit.Life, Effectiveness.Strong },
            { Bit.Elec, Effectiveness.Weak },
        };
        private static readonly Dictionary<Bit, Effectiveness> TableLife = new()
        {
            { Bit.Fire, Effectiveness.Weak },
            { Bit.Ice, Effectiveness.Neutral },
            { Bit.Venom, Effectiveness.Neutral },
            { Bit.Life, Effectiveness.Neutral },
            { Bit.Elec, Effectiveness.Strong },
        };
        private static readonly Dictionary<Bit, Effectiveness> TableElec = new()
        {
            { Bit.Fire, Effectiveness.Neutral },
            { Bit.Ice, Effectiveness.Strong },
            { Bit.Venom, Effectiveness.Neutral },
            { Bit.Life, Effectiveness.Strong },
            { Bit.Elec, Effectiveness.Strong },
        };

        private static readonly Dictionary<Bit, Dictionary<Bit, Effectiveness>> Table = new()
        {
            { Bit.Fire, TableFire },
            { Bit.Ice, TableIce },
            { Bit.Venom, TableVenom },
            { Bit.Life, TableLife },
            { Bit.Elec, TableElec },
        };

        #endregion Tables

        /// <summary>
        /// Stores calculated element results.
        /// Used to prevent needing to do expensive calulations every time masks are tested.
        /// </summary>
        private static readonly Dictionary<string, int> Cache = [];

        /// <summary>
        /// Makes a key for storing/retriving a value from the cache.
        /// </summary>
        /// <param name="atkElems">Bits of the attacking element.</param>
        /// <param name="defElems">Bits of the defending element.</param>
        private static string MakeCacheKey(Bit atkElems, Bit defElems)
        {
            return $"{(int)atkElems}:{(int)defElems}";
        }
        private static void PrintTable()
        {
            for (int i = 0; i < ElementCount; i++)
            {
                Bit atkElem = (Bit)(1 << i);
                List<string> arr = [atkElem.ToString()];
                for (int j = 0; j < ElementCount; j++)
                {
                    Bit defElem = (Bit)(1 << j);
                    arr.Add($" {CalcStrength(atkElem, defElem)} ");
                }
                GD.Print([.. arr]);
            }
        }
        /// <summary>
        /// Claculates and stores the strength value between two element masks.
        /// </summary>
        /// <param name="atkElems">Bits of the attacking element.</param>
        /// <param name="defElems">Bits of the defending element.</param>
        public static int CalcStrength(Bit atkElems, Bit defElems)
        {
            string cacheKey = MakeCacheKey(atkElems, defElems);
            if (Cache.TryGetValue(cacheKey, out int cacheValue))
            {
                return cacheValue;
            }

            int strengthValue = 0;
            for (int i = 0; i < ElementCount; i++)
            {
                Bit atkElem = (Bit)(1 << i);
                if ((atkElems & atkElem) > Bit.None)
                {
                    Dictionary<Bit, Effectiveness> elementTable = Table[atkElem];
                    for (int j = 0; j < ElementCount; j++)
                    {
                        Bit defElem = (Bit)(1 << j);
                        if ((defElems & defElem) > Bit.None)
                        {
                            strengthValue += (int)elementTable[defElem];
                        }
                    }
                }
            }

            if (!Engine.IsEditorHint())
            {
                Cache[cacheKey] = strengthValue;
            }
            return strengthValue;
        }
        /// <summary>
        /// Turns a strength value into a float between 0.0 and 2.0. With 1.0 being neutral.
        /// </summary>
        /// <param name="strength">The strength value to transform.</param>
        public static float StrengthToMult(int strength)
        {
            return 1f + (strength / (float)ElementCount);
        }

        #pragma warning disable CA1822 
        [ExportToolButton("Print Table")]
        private Callable ToolPrint => Callable.From(PrintTable);
        #pragma warning restore

        /// <summary>
        /// Used for the Godot interface.
        /// Set the elemental bit value of this object.
        /// </summary>
        [Export(PropertyHint.Flags, "Fire,Ice,Venom,Life,Electricity")]
        private int ElementValue
        {
            set
            {
                element = (Bit)value;
                EmitChanged();
            }
            get => (int)element;
        }
        private Bit element = Bit.None;

        /// <summary>
        /// Gets teh elemental value of this object.
        /// </summary>
        public Bit GetElement()
        {
            return element;
        }

        /// <summary>
        /// Gets this elements strength against another element.
        /// </summary>
        public int GetStrengthAgainst(Element defendingElement)
        {
            return CalcStrength(GetElement(), defendingElement.GetElement());
        }
    }
}