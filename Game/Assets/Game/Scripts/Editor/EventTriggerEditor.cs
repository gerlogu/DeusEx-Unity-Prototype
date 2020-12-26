using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EventTrigger))]
public class EventTriggerEditor : Editor
{
    EventType eventType = EventType.BATTLE_MUSIC;

    [MenuItem("Game Elements/EventSystem/EventTrigger")]
    private static void CreateEventTrigger()
    {
        Debug.Log("Object instantiated");
        Transform pos = SceneView.lastActiveSceneView.camera.transform;
        Instantiate(Resources.Load<GameObject>("Prefabs/EventTrigger"), pos.position + pos.forward * 17.5f, new Quaternion(0,0,0,0)).name = "EventTrigger";
    }

    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();

        var eventTrigger = target as EventTrigger;

        eventType = (EventType)EditorGUILayout.EnumPopup("Event Type", eventTrigger.eventType);

        eventTrigger.eventType = this.eventType;

        switch (eventType)
        {
            case EventType.BATTLE_MUSIC:
                eventTrigger.themeIndex = EditorGUILayout.IntSlider("Theme Index", eventTrigger.themeIndex, 1, 47);
                eventTrigger.musicTransitionSpeed = Mathf.Clamp(EditorGUILayout.FloatField("Transition Speed", eventTrigger.musicTransitionSpeed), 0, 10);
                eventTrigger.name = "Battle Music (Trigger)";
                break;

            case EventType.BACKGROUND_MUSIC:
                eventTrigger.themeIndex = EditorGUILayout.IntSlider("Theme Index", eventTrigger.themeIndex, 0, 6);
                eventTrigger.name = "Background Music (Trigger)";
                break;

            case EventType.START_BATTLE:
                eventTrigger.name = "Start Battle (Trigger)";
                break;

            case EventType.STOP_MUSIC:
                eventTrigger.musicTransitionSpeed = Mathf.Clamp(EditorGUILayout.FloatField("Stop Transition Speed", eventTrigger.musicTransitionSpeed), 0, 10);
                eventTrigger.name = "Stop Music (Trigger)";
                break;

            case EventType.DEATH:
                eventTrigger.name = "Death Area (Trigger)";
                break;
            case EventType.SUN_MODIFIER:
                eventTrigger.sunRotation = EditorGUILayout.Vector3Field("Rotation", eventTrigger.sunRotation);
                eventTrigger.name = "Sun Modifier Area (Trigger)";
                break;
        }

    }
}
