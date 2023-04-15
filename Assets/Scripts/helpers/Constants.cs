namespace KR
{
  public class Global
  {
    //
  }

  public class Tags
  {
    public const string PLAYER_TAG = "Player";
    public const string ENEMY_TAG = "Enemy";
    public const string HITTABLE_TAG = "Hittable";
    public const string INTERACTABLE_TAG = "Interactable";
  }

  public static class AnimationTags
  {
    public const string HORIZONTAL = "Horizontal";
    public const string VERTICAL = "Vertical";

    // Params
    public const string IS_INTERACTING_PARAM = "isInteracting";
    public const string CAN_DO_COMBO_PARAM = "canDoCombo";

    // Animations
    public const string LOCOMOTION = "Locomotion";
    public const string EMPTY = "Empty";
    public const string RIGHT_ARM_EMPTY = "Right Arm Empty";
    public const string LEFT_ARM_EMPTY = "Left Arm Empty";
    public const string ROLL_ANIMATION = "Roll";
    public const string SHIELD_ANIMATION = "Shield";
    public const string FALLING_ANIMATION = "Falling";
    public const string LAND_ANIMATION = "Land";
    public const string DAMAGE_LIGHT_ANIMATION = "Damage_Light";
    public const string DAMAGE_HEAVY_ANIMATION = "Damage_Heavy";
    public const string DEATH_ANIMATION = "Death";
    public const string PICK_UP_ITEM_ANIMATION = "Pick Up Item";
  }
}

