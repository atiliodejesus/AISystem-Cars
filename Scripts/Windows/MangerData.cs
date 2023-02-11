
using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.ShortcutManagement;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace ClockworkEditor
{
  /// <summary>
  ///     An editor tool mode (like Translate, Rotate, etc) that lets you click/drag to position objects in the scene.
  /// </summary>
  /// <remarks>Press and hold shift, then click and drag to place the object in the scene.</remarks>
  [EditorTool("Placer Tool")]
  public class ManagerData : EditorTool
  {
    private bool shiftHeld;
    private Vector3 mouseDownWorldPosition;
    private Vector3 mouseDragWorldPosition;
    private Vector3 mouseDownWorldNormal;
    private Plane mouseDownSurface;
    private Vector3 draggedForwardVector;
    private Quaternion orient;

    // This is called for each window that your tool is active in. Put the functionality of your tool here.
    public override void OnToolGUI(EditorWindow window)
    {
      if (shiftHeld)
      {
        // Forces this control to take priority and all other handles to be disabled.
        // We only take control on shift, so you can pick-select objects when it's unheld.
        HandleUtility.AddControl(0, 0);
      }

      Event e = Event.current;
      switch (e.type)
      {
        case EventType.KeyDown:
          if (e.keyCode == KeyCode.LeftShift)
          {
            shiftHeld = true;
          }

          break;
        case EventType.KeyUp:
          if (e.keyCode == KeyCode.LeftShift)
          {
            shiftHeld = false;
          }

          break;
        case EventType.MouseDown:
          if (e.button == 1)
          {
            return;
          }

          if (e.button == 0 && shiftHeld)
          {

            // A little hacky, but this is the only reliable way to ray cast into the scene without needing
            // colliders on all objects.

            object[] args = { Event.current.mousePosition, null };
            // ReSharper disable PossibleNullReferenceException
            Type type = Type.GetType(
                "UnityEditor.SceneViewMotion, UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            MethodInfo methodInfo =
                type.GetMethod("RaycastWorld", BindingFlags.Static | BindingFlags.NonPublic);
            bool didHit = (bool)methodInfo.Invoke(null, args);
            // ReSharper enable PossibleNullReferenceException

            if (didHit)
            {
              RaycastHit h = (RaycastHit)args[1];
              mouseDownWorldPosition = h.point;
              mouseDownWorldNormal = h.normal;
              mouseDownSurface = new Plane(h.normal, h.point);

              Undo.RecordObject(Selection.activeGameObject.transform,
                  $"Place Object ({Selection.activeGameObject.name})");
              Selection.activeGameObject.transform.position = mouseDownWorldPosition;
            }
          }

          break;
        case EventType.MouseDrag:
          if (e.button == 1)
          {
            return;
          }

          Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
          bool hit = mouseDownSurface.Raycast(ray, out float enter);
          if (hit)
          {
            mouseDragWorldPosition = ray.GetPoint(enter);
          }
          else
          {
            mouseDragWorldPosition =
                mouseDownSurface.ClosestPointOnPlane(ray.origin + ray.direction * 1000);
          }

          draggedForwardVector = (mouseDragWorldPosition - mouseDownWorldPosition).normalized;
          orient = Quaternion.LookRotation(draggedForwardVector, mouseDownWorldNormal);

          Undo.RecordObject(Selection.activeGameObject.transform,
              $"Place Object ({Selection.activeGameObject.name})");
          Selection.activeGameObject.transform.rotation = orient;

          break;
      }

      Handles.DotHandleCap(0, mouseDownWorldPosition, Quaternion.identity,
          HandleUtility.GetHandleSize(mouseDownWorldPosition) * .05f,
          EventType.Repaint);
      Handles.color = Color.white;
      Handles.DrawLine(mouseDownWorldPosition, mouseDragWorldPosition, 1.5f);
      Handles.color = Color.blue;
      Handles.DrawLine(mouseDownWorldPosition, mouseDownWorldPosition + draggedForwardVector * .2f, 3);
      Handles.color = Color.white;
    }

    [Shortcut("Clockwork/Place Object Tool", KeyCode.Y)]
    private static void Shortcut()
    {
      ToolManager.SetActiveTool(typeof(ManagerData));
    }
  }
}