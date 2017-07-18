import React, {Component} from 'react'
import MyMenu from '../components/MyMenu'
import BackButton from '../components/GoBack'
import {Button} from 'antd';
import FreeScrollBar from 'react-free-scrollbar';
export default class EmptyAppContainer extends Component {
  constructor(){
    super()
  }

  render() {
    return (
      <div className='allenbox'>
          {this.props.children}
      </div>
    )
  }
}
