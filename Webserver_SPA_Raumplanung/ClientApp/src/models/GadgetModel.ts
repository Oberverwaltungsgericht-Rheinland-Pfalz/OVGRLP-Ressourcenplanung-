import { Model } from '@vuex-orm/core'

export default class Gadget extends Model {
  // This is the name used as module name of the Vuex Store.
  public static entity = 'gadgets'

  // List of all fields (schema) of the post model. `this.attr` is used
  // for the generic field type. The argument is the default value.
  public static fields () {
    return {
      id: this.attr(null),
      title: this.attr(''),
      suppliedBy: this.attr(null)
    }
  }
}

export interface GadgetModel {
  id: string
  title: string
  suppliedBy: string
}
