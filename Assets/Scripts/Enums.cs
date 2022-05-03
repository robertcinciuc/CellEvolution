using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocalPlanes{
    CURRENT_PLANE, X_PLANE, Z_PLANE, XZ_PLANE
}

public enum Foods {
    Meat
}

public enum Enemies {
    OG_ENEMY
}

public enum Positions {
    UPPER, LOWER, RIGHT, LEFT, UPPER_RIGHT, UPPER_LEFT, LOWER_LEFT, LOWER_RIGHT, INSIDE, OUTSIDE
}

public enum BodyPartTypes {
    Mouths, LocomotionOrgans, AttackOrgans, Bodies
}

public enum Mouths {
    Mouth, MouthClaw
}

public enum LocomotionOrgans {
    Flagella, TwinFlagella
}

public enum AttackOrgans {
    Spike, Tooth
}

public enum Bodies {
    PlayerBody, OriginalEnemyBody
}

public enum Characters {
    Player, OriginalEnemy
}
