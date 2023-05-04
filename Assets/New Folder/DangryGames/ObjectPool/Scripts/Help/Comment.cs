using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comment : MonoBehaviour
{
    [SerializeField]
    [TextArea(6, 8)]
    private string comment;
}
