namespace lcLLM.Helpers
{
    internal class RefactorSuggestionHelper
    {
        private static Lazy<RefactoringHelper> _instance =
              new Lazy<RefactoringHelper>(() => new RefactoringHelper(), true);

        public static RefactoringHelper Instance => _instance.Value;

        public static void Reset()
        {
            _instance = new Lazy<RefactoringHelper>(() => new RefactoringHelper(), true);
        }
    }

}


