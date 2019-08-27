import React from 'react';
class AutoSizer extends React.Component {
    constructor() {
        super(...arguments);
        this.state = { height: 0, width: 0 };
        this.containerRef = null;
        this.timer = null;
        this.listenResize = () => {
            clearTimeout(this.timer);
            this.timer = setTimeout(() => {
                this.setSize();
            }, 100);
        };
        this.setSize = () => {
            if (this.containerRef) {
                const { clientHeight, clientWidth } = this.containerRef;
                this.setState({ height: clientHeight, width: clientWidth });
            }
        };
        this.setRef = (ref) => {
            this.containerRef = ref;
            this.setSize();
        };
        this.bindOrUnbindResize = (type) => {
            const listener = type === 'bind' ? window.addEventListener : window.removeEventListener;
            listener('resize', this.listenResize, false);
        };
    }
    componentDidMount() {
        this.bindOrUnbindResize('bind');
    }
    componentWillUnmount() {
        this.bindOrUnbindResize('unbind');
    }
    render() {
        const { className, style, children } = this.props;
        const { width, height } = this.state;
        return (React.createElement("div", { className: className, style: style, ref: this.setRef }, children({ width, height })));
    }
}
export default AutoSizer;
//# sourceMappingURL=index.js.map