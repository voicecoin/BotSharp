import React from 'react'

export class NoData extends React.Component{
    render(){
        let Contentattributes ={
            text: this.props.attribute.text,
            imgSrc : this.props.attribute.imgSrc
        }
        return(
            <div className='NoData'>                
                <div className='NoDataContent'>
                     <img src={Contentattributes.imgSrc}/>
                    <ul>
                        <li>{Contentattributes.text}</li>
                    </ul>
                </div>
            </div>
        )
    }
}
export default NoData