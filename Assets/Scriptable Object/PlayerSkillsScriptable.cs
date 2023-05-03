using UnityEngine;

[CreateAssetMenu(fileName = "New player skill", menuName = "Player Skill")]
public class PlayerSkillsScriptable : ScriptableObject
{
    [SerializeField] PlayerMovement playerMove;
    [SerializeField] FieldOfView fov;
    bool isView360;

    //public void FullAngleView()
    //{
    //    if (Input.GetKeyDown(KeyCode.F))
    //        isView360 = !isView360;

    //    if (isView360)
    //        fov.viewAngle = Mathf.Lerp(fov.viewAngle, 360, 0.125f);
    //    else
    //        fov.viewAngle = Mathf.Lerp(fov.viewAngle, 90, 0.125f);
    //}

} 
