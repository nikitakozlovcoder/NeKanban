namespace NeKanban.Common.Constants;

public enum PreferenceType
{
    Normal, 
    Favourite
}

public static class PreferenceTypeConstraints
{
    public static readonly PreferenceType[] ExclusivePreferenceTypes = {PreferenceType.Favourite};
}