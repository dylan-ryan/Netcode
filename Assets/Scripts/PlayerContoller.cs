using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private GameObject chatBox;
    private TextMeshProUGUI chatText;
    private bool isChatting = false;
    private string currentMessage = "";

    private NetworkVariable<Vector2> networkMovement = new NetworkVariable<Vector2>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!IsOwner) return;

        HandleMovement();
        HandleChatInput();
    }

    void FixedUpdate()
    {
        if (IsOwner)
        {
            SendMovementServerRpc(movement);
        }

        rb.velocity = networkMovement.Value * speed;
    }

    void HandleMovement()
    {
        if (isChatting) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        movement = new Vector2(moveX, moveY).normalized;
    }

    void HandleChatInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!isChatting)
            {
                StartChat();
                movement = Vector2.zero;
            }
            else
            {
                SendChatMessageServerRpc(currentMessage);
                EndChat();
            }
        }

        if (isChatting && Input.inputString.Length > 0)
        {
            currentMessage += Input.inputString;
            chatText.text = currentMessage;
        }
    }

    void StartChat()
    {
        chatBox.SetActive(true);
        chatText.text = "";
        currentMessage = "";
        isChatting = true;
        StopAllCoroutines();
    }

    void EndChat()
    {
        isChatting = false;
        StartCoroutine(ChatTimer());
    }

    private IEnumerator ChatTimer()
    {
        yield return new WaitForSeconds(5);
        chatBox.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        chatBox = transform.Find("ChatBox").gameObject;
        chatText = transform.Find("ChatBox/ChatText").GetComponent<TextMeshProUGUI>();
        chatBox.SetActive(false);
    }

    [ServerRpc]
    private void SendMovementServerRpc(Vector2 movement, ServerRpcParams rpcParams = default)
    {
        networkMovement.Value = movement;
        UpdateMovementClientRpc(movement);
    }

    [ClientRpc]
    private void UpdateMovementClientRpc(Vector2 movement)
    {
        if (!IsOwner)
        {
            networkMovement.Value = movement;
        }
    }

    [ServerRpc]
    private void SendChatMessageServerRpc(string message, ServerRpcParams rpcParams = default)
    {
        UpdateChatClientRpc(message);
    }

    [ClientRpc]
    private void UpdateChatClientRpc(string message)
    {
        if (chatBox != null)
        {
            chatBox.SetActive(true);
            chatText.text = message;
            StopAllCoroutines();
            StartCoroutine(ChatTimer());
        }
    }
}
