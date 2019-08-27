export var SOCKET_TYPE;
(function (SOCKET_TYPE) {
    SOCKET_TYPE["SOCKETIO"] = "socket.io";
    SOCKET_TYPE["WEBSOCKET"] = "websocket";
})(SOCKET_TYPE || (SOCKET_TYPE = {}));
export const SOCKER_TYPES = [SOCKET_TYPE.SOCKETIO, SOCKET_TYPE.WEBSOCKET];
export var DATA_FORMAT_TYPE;
(function (DATA_FORMAT_TYPE) {
    DATA_FORMAT_TYPE["JSON"] = "json";
    DATA_FORMAT_TYPE["TEXT"] = "text";
})(DATA_FORMAT_TYPE || (DATA_FORMAT_TYPE = {}));
export const DATA_FORMATS = [DATA_FORMAT_TYPE.JSON, DATA_FORMAT_TYPE.TEXT];
//# sourceMappingURL=socket.js.map