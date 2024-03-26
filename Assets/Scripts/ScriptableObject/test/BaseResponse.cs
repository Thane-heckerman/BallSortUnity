using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class BaseResponse : UnityEvent, IEventResponse // kế thừa 1 interface IeventRespone để define type
{
}

public class BaseResponse<TType> : UnityEvent<TType>, IEventResponse
{
}
