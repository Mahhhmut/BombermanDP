using Unity.Netcode.Components;
using UnityEngine;

[DisallowMultipleComponent]
public class ClientNetworkTransform : NetworkTransform
{
    // Bu metodun 'false' dönmesi, yetkinin Client'ta olmasını sağlar
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}