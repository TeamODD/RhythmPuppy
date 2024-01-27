using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patterns
{
    public class PatternBase : MonoBehaviour
    {
        /* 패턴 실행 정보만을 저장/전달하는 스크립트 */
        public PatternInfo patternInfo { get; set; }
    }
}