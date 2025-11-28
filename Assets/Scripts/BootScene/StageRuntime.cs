using System.Collections.Generic;

public class StageRuntime
{
    public string StageId { get; private set; }      // 이 스테이지의 ID
    public string CurrentRoomId { get; private set; } // 현재 플레이어가 있는 방 ID

    // 여기부터 구현부

    // TODO: 방 상태를 저장하는 딕셔너리
    // key: roomId, value: 그 방의 저장 상태(RoomState)
    // private Dictionary<string, RoomState> roomStates = new Dictionary<string, RoomState>();

    public StageRuntime(string stageId)
    {
        StageId = stageId;
        
        // TODO: 스테이지 시작 시 초기화해야 하는 것들 있으면 여기서 처리
        // ex) 스테이지 공용 랜덤 시드, 스테이지 진행도, 클리어 여부 등
    }

    public void SetCurrentRoom(string roomId)
    {
        CurrentRoomId = roomId;
    }

    // TODO: 방 상태를 가져오거나 없으면 새로 만드는 함수
    /*
    public RoomState GetOrCreateRoomState(string roomId)
    {
        if (!roomStates.TryGetValue(roomId, out var state))
        {
            state = new RoomState(roomId);
            roomStates.Add(roomId, state);
        }
        return state;
    }
    */

    // TODO: 나중에 세이브/로드까지 고려하면,
    // roomStates를 직렬화/역직렬화하는 함수들도 여기 들어올 수 있음
}
