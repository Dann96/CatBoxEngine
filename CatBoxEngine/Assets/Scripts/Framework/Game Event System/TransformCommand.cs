using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEventSystem
{
    public class TransformCommand : GameEventCommand
    {
        public Transform transform;
        public bool instantMove = false;
        public float speed = 10f;
        public bool rotate = false;
        public bool rotateTo = false;
        public bool[] useRotationAxes = new bool[3];
        public float[] rotationAxes = new float[3];
        public bool translate = false;
        public bool translateTo = false;
        public bool[] useTranslationAxes = new bool[3];
        public float[] translationAxes = new float[3];

        public override void InEditorUpdate(GameObject go) { }

        protected IEnumerator ExecuteCoroutine()
        {
            float rotX = useRotationAxes[0] ? rotationAxes[0] : (rotateTo ? transform.eulerAngles.x : 0);
            float rotY = useRotationAxes[1] ? rotationAxes[1] : (rotateTo ? transform.eulerAngles.y : 0);
            float rotZ = useRotationAxes[2] ? rotationAxes[2] : (rotateTo ? transform.eulerAngles.z : 0);

            float moveX = useTranslationAxes[0] ? translationAxes[0] : (translateTo ? transform.position.x : 0);
            float moveY = useTranslationAxes[1] ? translationAxes[1] : (translateTo ? transform.position.y : 0);
            float moveZ = useTranslationAxes[2] ? translationAxes[2] : (translateTo ? transform.position.z : 0);

            Quaternion startRot = transform.rotation;
            Vector3 startPos = transform.position;

            bool rotated = false;
            bool translated = false;
            float t = 0;
            while (!rotated || !translated)
            {
                if (rotate)
                {
                    Quaternion newRot = Quaternion.identity;
                    if (rotateTo)
                    {
                        newRot = Quaternion.Euler(rotX, rotY, rotZ);
                    }
                    else
                    {
                        newRot = startRot * Quaternion.Euler(rotX, rotY, rotZ);
                    }
                    if ((instantMove && transform.rotation != newRot) || (!instantMove && t < 1f))
                    {
                        if (instantMove)
                            transform.rotation = newRot;
                        else
                        {
                            transform.rotation = Quaternion.Slerp(startRot, newRot, t);
                        }
                            
                    }
                    else
                    {
                        rotated = true;
                    }
                    
                }
                else rotated = true;
                if (translate)
                {
                    Vector3 newMove = translateTo ? new Vector3(moveX, moveY, moveZ) : new Vector3(startPos.x + moveX, startPos.y + moveY, startPos.z + moveZ);
                    if (transform.position != newMove)
                    {
                        if (instantMove)
                            transform.position = newMove;
                        else
                            transform.position = Vector3.Lerp(startPos, newMove, t);
                    }
                    else
                    {
                        translated = true;
                    }
                }
                else translated = true;
                t += Time.deltaTime * speed;
                t = Mathf.Clamp01(t);
                yield return null;
            }
            finished = true;
        }

        protected override void ExecuteImmediately()
        {
            mono.StartCoroutine(ExecuteCoroutine());
        }
    }
}