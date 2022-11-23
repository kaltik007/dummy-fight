namespace Game.Utils.Saving
{
    public interface ISavable<T> where T : SaveParams
    {
        void ApplyParams(T? @params);

        T GetCurrentSaveParams();
    }
    
    public abstract record SaveParams
    {
        public abstract string ToJson();

        public abstract SaveParams FromJson(string json);

        public SaveParams()
        {
        }
    }
}