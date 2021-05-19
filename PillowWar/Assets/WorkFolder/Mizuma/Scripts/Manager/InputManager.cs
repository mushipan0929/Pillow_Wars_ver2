using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputManager : SingletonMonoBehaviour<InputManager>
{
    [SerializeField] private bool isUseKeyboard = false;
    [SerializeField] private int keyboardMovePlayerId;

    [SerializeField] private InputData[] playerInput;

    private CharacterMover characterMover = new CharacterMover();
    private GameManager gameManager;

    public MoveData moveData;
    public List<CharacterData> characterDatas = new List<CharacterData>();

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void UpdateMethod()
    {

    }

    public void GeneralInputUpdateMethod()
    {
        // オプション画面
        for (int i = 0; i < gameManager.joinPlayers; i++)
        {
            if (Input.GetButtonDown(playerInput[i].Option)) gameManager.isPause = !gameManager.isPause;
        }

        if (isUseKeyboard == true) KeyboardGeneralInputMethod();
    }

    public void UiInputUpdateMethod()
    {
        for (int i = 0; i < gameManager.joinPlayers; i++)
        {

        }

        if (isUseKeyboard == true) KeyboardUiInputUpdateMethod();
    }

    public void MoveInputUpdateMethod()
    {
        for (int i = 0; i < gameManager.joinPlayers; i++)
        {
            if (characterDatas[i].isDeath == true) continue;

            Vector3 moveInput = playerInput[i].MoveAxis;
            Vector3 viewMoveInput = playerInput[i].ViewPointMoveAxis;

            // 移動/ダッシュ解除判定
            if (moveInput.magnitude > 0.2f)
            {
                if(characterDatas[i].isRun) characterMover.Move(moveInput * moveData.dashMovMulti, characterDatas[i]);
                else characterMover.Move(moveInput, characterDatas[i]);
            }
            else characterDatas[i].isRun = false;

            // ダッシュ判定
            if (Input.GetButtonDown(playerInput[i].Run))
            {
                characterDatas[i].isRun = true;
            }
            // 視点移動
            if (viewMoveInput.magnitude > 0.05f)
            {
                characterMover.ViewMove(viewMoveInput, characterDatas[i]);
            }
            // ジャンプ
            if (Input.GetButtonDown(playerInput[i].Jump) && characterDatas[i].canJump == true)
            {
                characterMover.Jump(characterDatas[i]);
            }
            // ADS/非ADS
            if (Input.GetAxis(playerInput[i].SwitchToADS) > 0.2f)
            {
                characterMover.ToADS(characterDatas[i]);
            }
            else
            { 
                characterMover.ToNonADS(characterDatas[i]);
            }
            // 枕投げ
            if (Input.GetAxis(playerInput[i].PillowThrow) > 0.2f && characterDatas[i].remainthrowCT < 0 && characterDatas[i].isHavePillow)
            {
                characterMover.PillowThrow(characterDatas[i]);
            }
        }

        if (isUseKeyboard == true) KeyboardMoveInputUpdateMethod();
    }

    // ※テスト用※ キーボード操作
    private void KeyboardGeneralInputMethod()
    {
        // キーボード-オプション画面
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.isPause = !gameManager.isPause;
        }
    }

    private void KeyboardUiInputUpdateMethod()
    {

    }

    private void KeyboardMoveInputUpdateMethod()
    {
        CharacterData c = characterDatas[keyboardMovePlayerId];
        if (c.isDeath == true) return;

        // キーボード-視点移動
        KeyboardInputViewMove(c);

        if (Input.anyKey == false) return;
        // キーボード-移動
        KeyboardInputMove(c);
        // キーボード-ダッシュ判定/解除判定
        if (Input.GetKey(KeyCode.LeftShift)) c.isRun = true;
        else c.isRun = false;
        // キーボード-ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && c.canJump == true) characterMover.Jump(c);
        // キーボード-ADS/非ADS
        if (Input.GetMouseButton(1)) characterMover.ToADS(c);
        else characterMover.ToNonADS(c);
        // キーボード-枕投げ
        if (Input.GetKeyDown(KeyCode.F) && c.isHavePillow) characterMover.PillowThrow(c);
    }

    private void KeyboardInputMove(CharacterData c)
    {
        float spdMulti = (c.isRun == true) ? moveData.dashMovMulti : 1;

        if (Input.GetKey(KeyCode.W)) characterMover.Move(spdMulti * Vector3.forward, c);
        if (Input.GetKey(KeyCode.A)) characterMover.Move(spdMulti * Vector3.left, c);
        if (Input.GetKey(KeyCode.S)) characterMover.Move(spdMulti * Vector3.back, c);
        if (Input.GetKey(KeyCode.D)) characterMover.Move(spdMulti * Vector3.right, c);
    }

    private void KeyboardInputViewMove(CharacterData c)
    {
        Vector3 vec = new Vector3(Input.GetAxis("Mouse X"), 0f, Input.GetAxis("Mouse Y"));
        characterMover.ViewMove(vec, c);
    }
}
