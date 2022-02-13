namespace NeKanban.Constants;

public enum PreferenceType
{
    Normal, 
    Favourite
}

public static class PreferenceTypeConstraints
{
    public static PreferenceType[] ExclusivePreferenceTypes = {PreferenceType.Favourite};
}