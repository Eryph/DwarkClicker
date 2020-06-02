namespace Engine.Utils
{
    using System;

    [Serializable]
    public struct IntUpgrade
    {
		public string name;
		public string desc;
        public int value;
        public int cost;
		public float coef;
        public int max;
    };

    [Serializable]
    public struct FloatUpgrade
    {
		public string name;
		public string desc;
		public float value;
        public int cost;
		public float coef;
        public int max;
	};
}