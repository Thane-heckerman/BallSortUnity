using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class BaseResponse : UnityEvent, IEventResponse // kế thừa 1 interface IeventRespone để define type
{
}
[System.Serializable]
public class BaseResponse<TType> : UnityEvent<TType>, IEventResponse
{
}
