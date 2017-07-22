import React from 'react';
import {Link, IndexLink} from 'react-router';
// 引入Antd的导航组件
import { Menu, Icon, Switch } from 'antd';
import {DataURL} from '../config/DataURL-Config';
const SubMenu = Menu.SubMenu;
import Http from './XmlHttp';
const http = new Http();

class MyMenu extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            current: '1',
            menu : null
        }
    }
    fetchFn = () => {
            http.HttpAjax({
                url: DataURL + '/api/Menu',
                headers: {'authorization':'Bearer ' + localStorage.getItem('access_token')}
            }).then((data)=>{
              this.setState({menu:data});
            }).catch((e)=>{
                console.log(e.message)
            })
    }
    componentDidMount() {
        this.fetchFn();
    }
    handleClick = (e) => {
        this.setState({
            current: e.key
        })
    }

    render() {
        const menu = this.state.menu && this.state.menu.map(function(first){
          if(first.items && first.items.length > 0){
            return (
              <SubMenu key={first.id} title={<span><Icon type={first.icon} /><span>{first.name}</span></span>}>
                {
                  first.items.map(function(second){
                    if(second.items && second.items.length > 0){
                      return (
                        <SubMenu key={second.id} title={<span><Icon type={second.icon} /><span>{second.name}</span></span>}>
                          {
                            second.items.map(function(third){
                              return (
                                  third.dataUrl ?
                                  <Menu.Item key={third.id}><Link to={{pathname:third.link, query:third.dataUrl}}><Icon type={third.icon} />{third.name}</Link></Menu.Item>
                                  :
                                  <Menu.Item key={third.id}><Link to={third.link}><Icon type={third.icon} />{third.name}</Link></Menu.Item>
                              )
                            })
                          }
                        </SubMenu>
                      )
                    }
                    else{
                      return (
                          second.dataUrl ? <Menu.Item key={second.id}><Link to={{pathname:second.link, query:second.dataUrl}}><Icon type={second.icon} />{second.name}</Link></Menu.Item>
                          :
                          <Menu.Item key={second.id}><Link to={second.link}><Icon type={second.icon} />{second.name}</Link></Menu.Item>
                      )
                    }
                  })
                }
              </SubMenu>
            )
          }
          else{
            return (
                first.dataUrl ? <Menu.Item key={first.id}><Link to={{pathname:first.link, query:first.dataUrl}}><Icon type={first.icon} />{first.name}</Link></Menu.Item>
                :
                <Menu.Item key={first.id}><Link to={first.link}><Icon type={first.icon} />{first.name}</Link></Menu.Item>
            )
          }
        }
        );
        return (
                <div className="leftMenu">
                    <img src={require('../Sources/images/logo.png')} width="50" className="logo"/>
                    <Menu theme="dark"
                        onClick={this.handleClick}
                        style={{ width: 239 }}
                        defaultOpenKeys={['sub1']}
                        defaultSelectedKeys={[this.state.current]}
                        mode="inline"
                    >
                    {menu}
                    </Menu>
                </div>
        )
    }
}


export default MyMenu
