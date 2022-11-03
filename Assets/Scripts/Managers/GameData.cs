using System;
using System.Reflection;

[System.Serializable]
public class GameData
{
    //Player
    public float playerMoveSpeed = 1f;
    public float playerDashCooldown = 1f;
    public float playerHPMax = 1f;
    public float playerHPRegen = 1f;
    public float playerLuck = 1f;

    //Gun
    public float gunDamage = 1f;
    public float gunFireRate = 1f;
    public float gunReloadSpeed = 1f;
    public float gunMagSize = 1f;
    public float gunRange = 1f;

    public object GetFieldValue(string fieldName)
    {
        FieldInfo info = GetFieldInfo(fieldName);
        return info.GetValue(this);
    }

    public T GetFieldValue<T>(string fieldName)
    {
        return (T)GetFieldValue(fieldName);
    }

    public void SetFieldValue(string fieldName, object newValue)
    {
        FieldInfo info = GetFieldInfo(fieldName);
        info.SetValue(this, newValue);
    }

    // TODO: We probably don't need this, But I could be very wrong
    // My gut says, because the FieldInfo.SetValue only needs to take in an object, 
    //      I don't think we need a generic method that has a parameter of type <T>
    public void SetFieldValue<T>(string fieldName, T newValue)
    {
        SetFieldValue(fieldName, newValue);
    }

    // TODO: we need a tryGet and trySet

    private FieldInfo GetFieldInfo(string fieldName)
    {
        Type type = this.GetType();
        FieldInfo info = type.GetField(fieldName);

        if (info == null)
            throw new Exception($"Cannot find field name: {fieldName}");

        return info;
    }
}