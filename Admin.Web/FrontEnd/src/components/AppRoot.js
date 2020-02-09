var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
import * as React from "react";
var AppRoot = /** @class */ (function (_super) {
    __extends(AppRoot, _super);
    function AppRoot(p) {
        return _super.call(this, p) || this;
    }
    AppRoot.prototype.render = function () {
        return "Hello World";
    };
    return AppRoot;
}(React.Component));
export { AppRoot };
