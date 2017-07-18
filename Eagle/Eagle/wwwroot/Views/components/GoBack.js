import React from 'react'
import { browserHistory } from 'react-router'
import { Button, notification ,Icon} from 'antd';

export class BackButton extends React.Component{
    constructor(props){
        super(props)

    }

    goBack = ()=>{
        let  path = browserHistory.getCurrentLocation().hash;
        if(path == '#/index'){
            this.openNotificationWithIcon('warning')
        }else if(path == '#/'){
            this.openNotificationWithIcon('warning')
        }else{
            browserHistory.goBack()
        }
    }

    openNotificationWithIcon = (type) => {
        notification[type]({
            message: 'Warm reminder',
            description: 'Is now on the home page',
            duration:2
        });
    };
    render(){
        return(
         <Button type="omitted" className='goBack' onClick={this.goBack}>
            <Icon type="caret-left" />Back
         </Button>
        )
    }

}
export default BackButton
