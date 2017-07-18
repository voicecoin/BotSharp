import React from 'react';
import {Button} from 'antd'
import  {getUrlParams} from '../../../components/Utils.js';
import {DataURL} from '../../../config/DataURL-Config'
import Http from '../../../components/XmlHttp'
const http = new Http();
const bundleId=''
 export default class Entities extends React.Component{
     constructor(props){
         super(props)
         this.state={
             tData:[]
         }
         let wurl = window.location.href
         let NewbundleId =getUrlParams(wurl).bundleId;
         let newEntityName =getUrlParams(wurl).entityName
         this.geturl(NewbundleId,newEntityName);
     }

     gotoRecordAdd=()=>{
        let path = `/Structure/Bundles/AddRecord?bundleId=${this.bundleId}`
        this.context.router.push(path)
     }

     geturl(newbundleId,newEntityName){
        this.bundleId= newbundleId
        this.getEntitiesfetchFn(newbundleId,newEntityName)
     }

     getEntitiesfetchFn = (newbundleId,newEntityName) => {
        http.HttpAjax({
            url : DataURL + '/api/' + newEntityName + '/' + newbundleId
        }).then((data)=>{
            this.setState({tData:data})
        }).catch((e) => {
             console.log(e.message)
        })
     }


     render(){
         return(
             <div className='table'>
                <Button type="primary" onClick={this.gotoRecordAdd} >Add Record</Button>
             </div>
         )
     }

 }
    Entities.contextTypes = {
      router: React.PropTypes.object
    }
