using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public GameObject target = null;

    float startFOV = 110;
    float endFOV = 50;
    float goalTime = 3.0f;
    float deltaTime;

    void Start()
    {
        //MainCamera取得する。
        Camera camera = Camera.main;
    }

    void Update()
    {
        //カメラがターゲットに向き続ける処理。
        this.transform.LookAt(target.transform);
        //経過時間格納
        deltaTime += Time.deltaTime;
        //遂行率計算
        var fov = deltaTime / goalTime;
        //カメラをゆっくりズームさせる処理。
        Camera.main.fieldOfView = Mathf.Lerp(startFOV, endFOV, fov);
    }
}
