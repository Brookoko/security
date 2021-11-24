namespace security.Genetic
{
    public abstract class Key<T> where T : Key<T>
    {
        public abstract int Length { get; }

        public abstract void GetGenFrom(int index, T key);

        public virtual void AfterCrossover()
        {
        }

        public abstract void Mutate(int index);
    }
}
