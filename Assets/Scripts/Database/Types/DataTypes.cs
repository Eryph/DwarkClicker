namespace Engine.Utils
{
    using System;

    [Serializable]
    public struct IntUpgrade
    {
        public int value;
        public int cost;
		public float coef;
    };

    [Serializable]
    public struct FloatUpgrade
    {
        public float value;
        public int cost;
		public float coef;
	};
}